using ClosedXML.Excel;
using Hangfire;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.NhapDuLieu;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.NhapDuLieu;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class NhapDuLieuAdminService : INhapDuLieuAdmin
    {
        private readonly AppDbContext _context;
        private readonly IFileStorage _fileStorage;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public NhapDuLieuAdminService(
            AppDbContext context, 
            IFileStorage fileStorage,
            UserManager<AppUser> userManager,
            IWebHostEnvironment env)
        {
            _context = context;
            _fileStorage = fileStorage;
            _userManager = userManager;
            _env = env;
        }

        public async Task<PageResult<LichSuNhapDuLieuVM>> GetListLichSuNhapDuLieu(NhapDuLieuType type, int pageIndex = 1, int pageSize = 10, string? keyword = null, NhapDuLieuStatus? status = null)
        {
            var query = _context.LichSuNhapDuLieus.AsNoTracking()
                .Where(x => x.PhanLoai == type);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CreatedBy.Contains(keyword) || x.CreatedAt.ToString().Contains(keyword) || x.TongDuLieu.ToString().Contains(keyword));
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.TrangThai == status.Value);
            }

            var totalRecords = await query.CountAsync();

            var records = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new LichSuNhapDuLieuVM
                {
                    Id = x.Id,
                    TongDuLieu = x.TongDuLieu,
                    ThanhCong = x.ThanhCong,
                    ThatBai = x.ThatBai,
                    TrangThai = x.TrangThai,
                    PhanLoai = x.PhanLoai,
                    DuongDanFileGoc = x.DuongDanFileGoc,
                    DuongDanFileLoi = x.DuongDanFileLoi,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy
                })
                .ToListAsync();

            return new PageResult<LichSuNhapDuLieuVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            };
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task<BaseResult> ProcessSinhVienExcel(int id)
        {
            var importRecord = await _context.LichSuNhapDuLieus.FindAsync(id);
            if (importRecord == null)
                return BaseResult.Fail("Không tìm thấy bản ghi nhập dữ liệu");

            if (importRecord.TrangThai != NhapDuLieuStatus.ChoXuLy)
                return BaseResult.Fail("Bản ghi đã được xử lý");

            try
            {
                // Chuyển đường dẫn tương đối thành đường dẫn vật lý
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var relativePath = importRecord.DuongDanFileGoc.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
                var physicalPath = Path.Combine(webRoot, relativePath);

                if (!File.Exists(physicalPath))
                {
                    importRecord.TrangThai = NhapDuLieuStatus.Loi;
                    importRecord.GhiChu = $"Không tìm thấy file: {physicalPath}";
                    await _context.SaveChangesAsync();
                    return BaseResult.Fail($"Không tìm thấy file: {physicalPath}");
                }
                    

                using var workbook = new XLWorkbook(physicalPath);
                var worksheet = workbook.Worksheet(1);

                var excelToImport = worksheet
                    .RowsUsed()
                    .Skip(1)
                    .Select((row, index) => new SinhVienImportRow
                    {
                        RowNumber = index + 2,
                        HoTen = row.Cell(1).GetValue<string>().Trim(),
                        MaSV = row.Cell(2).GetValue<string>().Trim(),
                        NgaySinhRaw = row.Cell(3).GetValue<string>().Trim(),
                        Email = row.Cell(4).GetValue<string>().Trim(),
                        SDT = row.Cell(5).GetValue<string>().Trim()
                    })
                    .ToList();

                // Get existing MaSV from database to check duplicates
                var existingMaSVs = await _context.SinhViens
                    .Select(x => x.MaSV)
                    .ToListAsync();

                var maSVsInExcel = new HashSet<string>();

                // Validate each row
                foreach (var row in excelToImport)
                {
                    var errors = new List<string>();

                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(row.HoTen))
                        errors.Add("Họ tên không được để trống");

                    if (string.IsNullOrWhiteSpace(row.MaSV))
                        errors.Add("Mã sinh viên không được để trống");
                    else if (existingMaSVs.Contains(row.MaSV))
                        errors.Add("Mã sinh viên đã tồn tại trong hệ thống");
                    else if (maSVsInExcel.Contains(row.MaSV))
                        errors.Add("Mã sinh viên bị trùng lặp trong file");
                    else
                        maSVsInExcel.Add(row.MaSV);

                    if (string.IsNullOrWhiteSpace(row.NgaySinhRaw))
                        errors.Add("Ngày sinh không được để trống");
                    else if (!DateTime.TryParseExact(
                        row.NgaySinhRaw,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out _))
                    {
                        errors.Add("Ngày sinh không đúng định dạng (dd/MM/yyyy)");
                    };

                    if (string.IsNullOrWhiteSpace(row.Email))
                        errors.Add("Email không được để trống");
                    else if (!IsValidEmail(row.Email))
                        errors.Add("Email không đúng định dạng");

                    if (string.IsNullOrWhiteSpace(row.SDT))
                        errors.Add("Số điện thoại không được để trống");
                    else if (!IsValidPhoneNumber(row.SDT))
                        errors.Add("Số điện thoại không đúng định dạng (10-11 số)");

                    if (errors.Any())
                    {
                        row.IsValid = false;
                        row.ErrorMessage = string.Join("; ", errors);
                    }
                }

                var validRows = excelToImport.Where(x => x.IsValid).ToList();
                var invalidRows = excelToImport.Where(x => !x.IsValid).ToList();

                // Batch insert valid records
                var actuallySuccessful = 0;
                var actuallyFailed = invalidRows.Count;

                if (validRows.Any())
                {
                    var sinhViensToAdd = new List<SinhVien>();

                    foreach (var row in validRows)
                    {
                        // Tạo tài khoản người dùng
                        var appUser = new AppUser
                        {
                            UserName = row.MaSV, // Sử dụng Mã SV làm username
                            Email = row.Email,
                            EmailConfirmed = true,
                            HoTen = row.HoTen,
                            PhoneNumber = row.SDT
                        };

                        var createResult = await _userManager.CreateAsync(appUser, "Student@123"); // Password mặc định

                        if (createResult.Succeeded)
                        {
                            // Gán role Student
                            await _userManager.AddToRoleAsync(appUser, "Student");

                            // Tạo thông tin sinh viên
                            var sinhVien = new SinhVien
                            {
                                Id = Guid.NewGuid(),
                                HoTen = row.HoTen,
                                MaSV = row.MaSV,
                                NgaySinh = DateTime.ParseExact(
                                    row.NgaySinhRaw,
                                    "dd/MM/yyyy",
                                    CultureInfo.InvariantCulture
                                ),
                                Email = row.Email,
                                SDT = row.SDT,
                                NguoiDungId = appUser.Id,
                                TimViec = false,
                                CreatedAt = DateTime.Now,
                                CreatedBy = importRecord.CreatedBy
                            };

                            sinhViensToAdd.Add(sinhVien);
                            actuallySuccessful++;
                        }
                        else
                        {
                            // Nếu tạo tài khoản thất bại, thêm vào danh sách lỗi
                            row.IsValid = false;
                            row.ErrorMessage = $"Không thể tạo tài khoản: {string.Join(", ", createResult.Errors.Select(e => e.Description))}";
                            invalidRows.Add(row);
                            actuallyFailed++;
                        }
                    }

                    if (sinhViensToAdd.Any())
                    {
                        await _context.SinhViens.AddRangeAsync(sinhViensToAdd);
                        await _context.SaveChangesAsync();
                    }
                }

                // Generate error Excel if there are invalid rows
                string? errorFilePath = null;
                if (invalidRows.Any())
                {
                    errorFilePath = GenerateErrorSinhVienExcel(invalidRows, importRecord.DuongDanFileGoc);
                }

                // Update import record
                importRecord.TrangThai = NhapDuLieuStatus.DaXuLy;
                importRecord.TongDuLieu = excelToImport.Count;
                importRecord.ThanhCong = actuallySuccessful;
                importRecord.ThatBai = actuallyFailed;
                importRecord.DuongDanFileLoi = errorFilePath;
                await _context.SaveChangesAsync();

                return BaseResult.Success(actuallyFailed > 0
                    ? $"Đã xử lý: {actuallySuccessful} thành công, {actuallyFailed} thất bại" 
                    : "Nhập dữ liệu thành công");
            }
            catch (Exception ex)
            {
                importRecord.TrangThai = NhapDuLieuStatus.Loi;
                importRecord.GhiChu = ex.Message;
                await _context.SaveChangesAsync();
                return BaseResult.Fail(ex.Message);
            }
        }

        public async Task<BaseResult> UploadSinhVienExcel(IFormFile file, string uploadById)
        {
            if (file == null || file.Length == 0)
                return BaseResult.Fail("File không hợp lệ");

            // 1. Save file
            var uploadOptions = new FileUploadOptions
            {
                Folder = "uploads/import/sinh-vien",
                AllowedExtensions = new[] { ".xlsx"},
                MaxSizeInBytes = 10 * 1024 * 1024, // 10MB
                RenameFile = true
            };

            var uploadResult = await _fileStorage.UploadAsync(file, uploadOptions);
            if (!uploadResult.IsSuccess)
                return BaseResult.Fail(uploadResult.Message ?? "Upload file thất bại");

            // 2. Create history record
            var history = new LichSuNhapDuLieu
            {
                PhanLoai = NhapDuLieuType.SinhVien,
                DuongDanFileGoc = uploadResult.FilePath ?? string.Empty,
                TrangThai = NhapDuLieuStatus.ChoXuLy,
                CreatedAt = DateTime.Now,
                CreatedBy = uploadById
            };

            _context.LichSuNhapDuLieus.Add(history);
            await _context.SaveChangesAsync();

            // 3. Enqueue Hangfire job
            BackgroundJob.Enqueue<NhapDuLieuAdminService>(
                x => x.ProcessSinhVienExcel(history.Id)
            );

            return BaseResult.Success();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Remove spaces and common separators
            var cleanedPhone = Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");

            // Check if it's 10-11 digits
            return Regex.IsMatch(cleanedPhone, @"^(0|\+84)[0-9]{9,10}$");
        }

        private string GenerateErrorSinhVienExcel(List<SinhVienImportRow> invalidRows, string originalFilePath)
        {
            try
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fileName = $"SinhVien_Errors_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var relativeFolder = Path.Combine("uploads", "import", "sinh-vien", "errors");
                var folderPath = Path.Combine(webRoot, relativeFolder);
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Lỗi");

                // Add headers
                worksheet.Cell(1, 1).Value = "Họ tên";
                worksheet.Cell(1, 2).Value = "Mã SV";
                worksheet.Cell(1, 3).Value = "Ngày sinh";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "SĐT";
                worksheet.Cell(1, 6).Value = "Dòng dữ liệu";
                worksheet.Cell(1, 7).Value = "Lỗi";

                // Style headers
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Add error data
                var row = 2;
                foreach (var invalidRow in invalidRows)
                {
                    worksheet.Cell(row, 1).Value = invalidRow.HoTen;
                    worksheet.Cell(row, 2).Value = invalidRow.MaSV;
                    worksheet.Cell(row, 3).Value = invalidRow.NgaySinhRaw;
                    worksheet.Cell(row, 4).Value = invalidRow.Email;
                    worksheet.Cell(row, 5).Value = invalidRow.SDT;
                    worksheet.Cell(row, 6).Value = invalidRow.RowNumber;
                    worksheet.Cell(row, 7).Value = invalidRow.ErrorMessage;
                    
                    // Highlight error rows
                    worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.LightPink;
                    row++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(filePath);

                // Return relative path for web access
                var relativePath = "/" + Path.Combine(relativeFolder, fileName).Replace('\\', '/');
                return relativePath;
            }
            catch (Exception ex)
            {
                // Log error but don't fail the whole process
                Console.WriteLine($"Error generating error Excel: {ex.Message}");
                return null;
            }
        }

        public async Task<BaseResult> UploadDoanhNghiepExcel(IFormFile file, string uploadById)
        {
            if (file == null || file.Length == 0)
                return BaseResult.Fail("File không hợp lệ");

            // 1. Save file
            var uploadOptions = new FileUploadOptions
            {
                Folder = "uploads/import/doanh-nghiep",
                AllowedExtensions = new[] { ".xlsx" },
                MaxSizeInBytes = 10 * 1024 * 1024, // 10MB
                RenameFile = true
            };

            var uploadResult = await _fileStorage.UploadAsync(file, uploadOptions);
            if (!uploadResult.IsSuccess)
                return BaseResult.Fail(uploadResult.Message ?? "Upload file thất bại");

            // 2. Create history record
            var history = new LichSuNhapDuLieu
            {
                PhanLoai = NhapDuLieuType.DoanhNghiep,
                DuongDanFileGoc = uploadResult.FilePath ?? string.Empty,
                TrangThai = NhapDuLieuStatus.ChoXuLy,
                CreatedAt = DateTime.Now,
                CreatedBy = uploadById
            };

            _context.LichSuNhapDuLieus.Add(history);
            await _context.SaveChangesAsync();

            // 3. Enqueue Hangfire job
            BackgroundJob.Enqueue<NhapDuLieuAdminService>(
                x => x.ProcessDoanhNghiepExcel(history.Id)
            );

            return BaseResult.Success();
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task<BaseResult> ProcessDoanhNghiepExcel(int id)
        {
            var importRecord = await _context.LichSuNhapDuLieus.FindAsync(id);
            if (importRecord == null)
                return BaseResult.Fail("Không tìm thấy bản ghi nhập dữ liệu");

            if (importRecord.TrangThai != NhapDuLieuStatus.ChoXuLy)
                return BaseResult.Fail("Bản ghi đã được xử lý");

            try
            {
                // Chuyển đường dẫn tương đối thành đường dẫn vật lý
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var relativePath = importRecord.DuongDanFileGoc.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
                var physicalPath = Path.Combine(webRoot, relativePath);

                if (!File.Exists(physicalPath))
                {
                    importRecord.TrangThai = NhapDuLieuStatus.Loi;
                    importRecord.GhiChu = $"Không tìm thấy file: {physicalPath}";
                    await _context.SaveChangesAsync();
                    return BaseResult.Fail($"Không tìm thấy file: {physicalPath}");
                }

                using var workbook = new XLWorkbook(physicalPath);
                var worksheet = workbook.Worksheet(1);

                var excelToImport = worksheet
                    .RowsUsed()
                    .Skip(1)
                    .Select((row, index) => new DoanhNghiepImportRow
                    {
                        RowNumber = index + 2,
                        TenPhapLy = row.Cell(1).GetValue<string>().Trim(),
                        TenHienThi = row.Cell(2).GetValue<string>().Trim(),
                        MaDN = row.Cell(3).GetValue<string>().Trim(),
                        Website = row.Cell(4).GetValue<string>().Trim(),
                        MaSoThue = row.Cell(5).GetValue<string>().Trim(),
                        NgayThanhLapRaw = row.Cell(6).GetValue<string>().Trim(),
                        Email = row.Cell(7).GetValue<string>().Trim(),
                        SDT = row.Cell(8).GetValue<string>().Trim(),
                        DiaChi = row.Cell(9).GetValue<string>().Trim(),
                        QuyMoNhanSuRaw = row.Cell(10).GetValue<string>().Trim()
                    })
                    .ToList();

                // Get existing MaDN from database to check duplicates
                var existingMaDNs = await _context.DoanhNghieps
                    .Select(x => x.MaDN)
                    .ToListAsync();

                var maDNsInExcel = new HashSet<string>();

                // Validate each row
                foreach (var row in excelToImport)
                {
                    var errors = new List<string>();

                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(row.TenHienThi))
                        errors.Add("Tên hiển thị không được để trống");

                    if (string.IsNullOrWhiteSpace(row.MaDN))
                        errors.Add("Mã doanh nghiệp không được để trống");
                    else if (existingMaDNs.Contains(row.MaDN))
                        errors.Add("Mã doanh nghiệp đã tồn tại trong hệ thống");
                    else if (maDNsInExcel.Contains(row.MaDN))
                        errors.Add("Mã doanh nghiệp bị trùng lặp trong file");
                    else
                        maDNsInExcel.Add(row.MaDN);

                    if (string.IsNullOrWhiteSpace(row.TenPhapLy))
                        errors.Add("Tên pháp lý không được để trống");

                    // Validate NgayThanhLap if provided
                    if (!string.IsNullOrWhiteSpace(row.NgayThanhLapRaw))
                    {
                        if (!DateTime.TryParseExact(
                            row.NgayThanhLapRaw,
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out _))
                        {
                            errors.Add("Ngày thành lập không đúng định dạng (dd/MM/yyyy)");
                        }
                    }

                    if (string.IsNullOrWhiteSpace(row.Email))
                        errors.Add("Email không được để trống");
                    else if (!IsValidEmail(row.Email))
                        errors.Add("Email không đúng định dạng");

                    if (!string.IsNullOrWhiteSpace(row.SDT) && !IsValidPhoneNumber(row.SDT))
                        errors.Add("Số điện thoại không đúng định dạng (10-11 số)");

                    // Validate QuyMoNhanSu if provided
                    if (!string.IsNullOrWhiteSpace(row.QuyMoNhanSuRaw))
                    {
                        if (!int.TryParse(row.QuyMoNhanSuRaw, out var quyMo) || quyMo < 0)
                            errors.Add("Quy mô nhân sự phải là số nguyên dương");
                    }

                    if (errors.Any())
                    {
                        row.IsValid = false;
                        row.ErrorMessage = string.Join("; ", errors);
                    }
                }

                var validRows = excelToImport.Where(x => x.IsValid).ToList();
                var invalidRows = excelToImport.Where(x => !x.IsValid).ToList();

                // Batch insert valid records
                var actuallySuccessful = 0;
                var actuallyFailed = invalidRows.Count;

                if (validRows.Any())
                {
                    var doanhNghiepsToAdd = new List<DoanhNghiep>();

                    foreach (var row in validRows)
                    {
                        // Tạo tài khoản người dùng
                        var appUser = new AppUser
                        {
                            UserName = row.MaDN, // Sử dụng Mã DN làm username
                            Email = row.Email,
                            EmailConfirmed = true,
                            HoTen = row.TenHienThi,
                            PhoneNumber = row.SDT
                        };

                        var createResult = await _userManager.CreateAsync(appUser, "Company@123"); // Password mặc định

                        if (createResult.Succeeded)
                        {
                            // Gán role Company
                            await _userManager.AddToRoleAsync(appUser, "Company");

                            // Tạo thông tin doanh nghiệp
                            var doanhNghiep = new DoanhNghiep
                            {
                                Id = Guid.NewGuid(),
                                TenPhapLy = row.TenPhapLy,
                                TenHienThi = row.TenHienThi,
                                MaDN = row.MaDN,
                                Website = string.IsNullOrWhiteSpace(row.Website) ? null : row.Website,
                                MaSoThue = string.IsNullOrWhiteSpace(row.MaSoThue) ? null : row.MaSoThue,
                                NgayThanhLap = string.IsNullOrWhiteSpace(row.NgayThanhLapRaw)
                                    ? null
                                    : DateTime.ParseExact(
                                        row.NgayThanhLapRaw,
                                        "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture
                                    ),
                                Email = row.Email,
                                SDT = string.IsNullOrWhiteSpace(row.SDT) ? null : row.SDT,
                                DiaChi = string.IsNullOrWhiteSpace(row.DiaChi) ? null : row.DiaChi,
                                QuyMoNhanSu = string.IsNullOrWhiteSpace(row.QuyMoNhanSuRaw)
                                    ? null
                                    : int.Parse(row.QuyMoNhanSuRaw),
                                NguoiDungId = appUser.Id,
                                CreatedAt = DateTime.Now,
                                CreatedBy = importRecord.CreatedBy
                            };

                            doanhNghiepsToAdd.Add(doanhNghiep);
                            actuallySuccessful++;
                        }
                        else
                        {
                            // Nếu tạo tài khoản thất bại, thêm vào danh sách lỗi
                            row.IsValid = false;
                            row.ErrorMessage = $"Không thể tạo tài khoản: {string.Join(", ", createResult.Errors.Select(e => e.Description))}";
                            invalidRows.Add(row);
                            actuallyFailed++;
                        }
                    }

                    if (doanhNghiepsToAdd.Any())
                    {
                        await _context.DoanhNghieps.AddRangeAsync(doanhNghiepsToAdd);
                        await _context.SaveChangesAsync();
                    }
                }

                // Generate error Excel if there are invalid rows
                string? errorFilePath = null;
                if (invalidRows.Any())
                {
                    errorFilePath = GenerateErrorDoanhNghiepExcel(invalidRows, importRecord.DuongDanFileGoc);
                }

                // Update import record
                importRecord.TrangThai = NhapDuLieuStatus.DaXuLy;
                importRecord.TongDuLieu = excelToImport.Count;
                importRecord.ThanhCong = actuallySuccessful;
                importRecord.ThatBai = actuallyFailed;
                importRecord.DuongDanFileLoi = errorFilePath;
                await _context.SaveChangesAsync();

                return BaseResult.Success(actuallyFailed > 0
                    ? $"Đã xử lý: {actuallySuccessful} thành công, {actuallyFailed} thất bại"
                    : "Nhập dữ liệu thành công");
            }
            catch (Exception ex)
            {
                importRecord.TrangThai = NhapDuLieuStatus.Loi;
                importRecord.GhiChu = ex.Message;
                await _context.SaveChangesAsync();
                return BaseResult.Fail(ex.Message);
            }
        }

        private string GenerateErrorDoanhNghiepExcel(List<DoanhNghiepImportRow> invalidRows, string originalFilePath)
        {
            try
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fileName = $"DoanhNghiep_Errors_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var relativeFolder = Path.Combine("uploads", "import", "doanh-nghiep", "errors");
                var folderPath = Path.Combine(webRoot, relativeFolder);
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Lỗi");

                // Add headers
                worksheet.Cell(1, 1).Value = "Tên pháp lý";
                worksheet.Cell(1, 2).Value = "Tên hiển thị";
                worksheet.Cell(1, 3).Value = "Mã DN";
                worksheet.Cell(1, 4).Value = "Website";
                worksheet.Cell(1, 5).Value = "Mã số thuế";
                worksheet.Cell(1, 6).Value = "Ngày thành lập";
                worksheet.Cell(1, 7).Value = "Email";
                worksheet.Cell(1, 8).Value = "SĐT";
                worksheet.Cell(1, 9).Value = "Địa chỉ";
                worksheet.Cell(1, 10).Value = "Quy mô nhân sự";
                worksheet.Cell(1, 11).Value = "Dòng dữ liệu";
                worksheet.Cell(1, 12).Value = "Lỗi";

                // Style headers
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                // Add error data
                var row = 2;
                foreach (var invalidRow in invalidRows)
                {
                    worksheet.Cell(row, 1).Value = invalidRow.TenPhapLy;
                    worksheet.Cell(row, 2).Value = invalidRow.TenHienThi;
                    worksheet.Cell(row, 3).Value = invalidRow.MaDN;
                    worksheet.Cell(row, 4).Value = invalidRow.Website;
                    worksheet.Cell(row, 5).Value = invalidRow.MaSoThue;
                    worksheet.Cell(row, 6).Value = invalidRow.NgayThanhLapRaw;
                    worksheet.Cell(row, 7).Value = invalidRow.Email;
                    worksheet.Cell(row, 8).Value = invalidRow.SDT;
                    worksheet.Cell(row, 9).Value = invalidRow.DiaChi;
                    worksheet.Cell(row, 10).Value = invalidRow.QuyMoNhanSuRaw;
                    worksheet.Cell(row, 11).Value = invalidRow.RowNumber;
                    worksheet.Cell(row, 12).Value = invalidRow.ErrorMessage;

                    // Highlight error rows
                    worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.LightPink;
                    row++;
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(filePath);

                // Return relative path for web access
                var relativePath = "/" + Path.Combine(relativeFolder, fileName).Replace('\\', '/');
                return relativePath;
            }
            catch (Exception ex)
            {
                // Log error but don't fail the whole process
                Console.WriteLine($"Error generating error Excel: {ex.Message}");
                return null;
            }
        }
    }
}
