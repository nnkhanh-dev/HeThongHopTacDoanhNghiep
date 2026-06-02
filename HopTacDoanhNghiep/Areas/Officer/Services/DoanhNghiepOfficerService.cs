using System.Globalization;
using System.Text;
using HopTacDoanhNghiep.Areas.Officer.ViewModels;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.DonVi;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Officer.Services
{
    public class DoanhNghiepOfficerService : IDoanhNghiepOfficer
    {
        private readonly AppDbContext _context;

        public DoanhNghiepOfficerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<DangKyDoanhNghiepVM>> GetListDangKyDoanhNghiep(int pageIndex, int pageSize, string? keyword, string MaCB)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            // Lấy thông tin cán bộ
            var canBo = await _context.CanBos.Include(x => x.DonVi).AsNoTracking().FirstOrDefaultAsync(x => x.MaCB == MaCB);

            var isPCTSV = canBo.DonVi.TenDV == "Phòng Công Tác Sinh Viên";
            var isBGH = canBo.DonVi.TenDV == "Ban Giám Hiệu";
            var isKhoa = canBo.DonVi.NhanDoiTac == true;

            var query = _context.DoanhNghieps.AsNoTracking();

            if(isBGH)
            {
                query = query.Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.XacNhanHopTac);
            }

            if (isPCTSV)
            {
                query = query.Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.ChoXuLy);
            }

            if (isKhoa)
            {
                // Khoa không có phần quản lý hợp tác riêng
                return new PageResult<DangKyDoanhNghiepVM>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRecords = 0,
                    Records = new List<DangKyDoanhNghiepVM>()
                };
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TenPhapLy.Contains(keyword.Trim()) || x.MaSoThue.Contains(keyword.Trim()));
            }

            var totalRecords = await query.CountAsync();

            var records = await query.OrderByDescending(x => x.CreatedAt)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .Select(x => new DangKyDoanhNghiepVM
                                     {
                                         MaDN = x.MaDN,
                                         TenHienThi = x.TenHienThi,
                                         TenPhapLy = x.TenPhapLy,
                                         MaSoThue = x.MaSoThue,
                                         Website = x.Website,
                                         Hotline = x.Hotline,
                                         EmailCongTy = x.EmailCongTy,
                                         NoiDungHopTac = x.NoiDungHopTac,
                                         TrangThaiHopTac = x.TrangThaiHopTac,
                                         CreatedAt = x.CreatedAt,
                                         UpdatedAt = x.UpdatedAt
                                     }).ToListAsync();

            return new PageResult<DangKyDoanhNghiepVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
             };
        }

        public async Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string keyword)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var query = _context.DoanhNghieps.Where(x => x.TrangThaiHopTac == HopTacDoanhNghiepStatus.DuyetHopTac).AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                var k = keyword.Trim();
                query = query.Where(x => x.TenHienThi.Contains(k) || x.MaDN.Contains(k) || (x.MaSoThue != null && x.MaSoThue.Contains(k)));
            }

            var total = await query.CountAsync();

            var records = await query.OrderByDescending(x => x.CreatedAt)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .Select(x => new DoanhNghiepVM
                                     {
                                         MaDN = x.MaDN,
                                         TenHienThi = x.TenHienThi,
                                         TenPhapLy = x.TenPhapLy,
                                         MaSoThue = x.MaSoThue,
                                         Website = x.Website,
                                         Hotline = x.Hotline,
                                         EmailCongTy = x.EmailCongTy,
                                         DiaChi = x.DiaChi,
                                         GioiThieu = x.GioiThieu,
                                         QuyMoNhanSu = x.QuyMoNhanSu,
                                         NoiDungHopTac = x.NoiDungHopTac,
                                         HoTenNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.HoTen : null,
                                         SoDienThoaiNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.PhoneNumber : null,
                                         EmailNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.Email : null,
                                         AnhNguoiDaiDien = x.NguoiDung != null ? x.NguoiDung.AnhDaiDien : null,
                                         TrangThaiHopTac = x.TrangThaiHopTac,
                                         CreatedAt = x.CreatedAt,
                                         UpdatedAt = x.UpdatedAt
                                     }).ToListAsync();

            return new PageResult<DoanhNghiepVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total,
                Records = records
            };
        }

        public async Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN(string MaDN)
        {
            if (string.IsNullOrEmpty(MaDN))
                return BaseResult<DoanhNghiepVM>.Fail("Mã doanh nghiệp không hợp lệ");

            var item = await _context.DoanhNghieps
                                     .Include(x => x.NguoiDung)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.MaDN == MaDN);

            if (item == null)
                return BaseResult<DoanhNghiepVM>.Fail("Không tìm thấy doanh nghiệp");

            var vm = new DoanhNghiepVM
            {
                MaDN = item.MaDN,
                TenHienThi = item.TenHienThi,
                TenPhapLy = item.TenPhapLy,
                MaSoThue = item.MaSoThue,
                Website = item.Website,
                Hotline = item.Hotline,
                EmailCongTy = item.EmailCongTy,
                DiaChi = item.DiaChi,
                GioiThieu = item.GioiThieu,
                QuyMoNhanSu = item.QuyMoNhanSu,
                NoiDungHopTac = item.NoiDungHopTac,
                HoTenNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.HoTen : null,
                SoDienThoaiNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.PhoneNumber : null,
                EmailNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.Email : null,
                AnhNguoiDaiDien = item.NguoiDung != null ? item.NguoiDung.AnhDaiDien : null,
                TrangThaiHopTac = item.TrangThaiHopTac,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };

            return BaseResult<DoanhNghiepVM>.Success(vm);
        }

        // Hàm này chỉ dùng cho PCTSV và BGH
        public async Task<BaseResult> UpdateTrangThaiHopTac(string MaDN, HopTacDoanhNghiepStatus trangThai, string MaCB)
        {
            // Kiểm tra dữ liệu đầu vào hợp lệ
            if (string.IsNullOrEmpty(MaDN) || 
                string.IsNullOrEmpty(MaCB) || 
                !Enum.IsDefined(typeof(HopTacDoanhNghiepStatus), trangThai)
            )
            {
                return BaseResult.Fail("Dữ liệu không hợp lệ");
            }

            // Thông tin đăng ký doanh nghiệp
            var doanhNghiep = await _context.DoanhNghieps
                .Include(x => x.HopTacDonVis)
                .FirstOrDefaultAsync(x => x.MaDN == MaDN);

            if (doanhNghiep == null)
            {
                return BaseResult.Fail("Không tìm thấy doanh nghiệp");
            }

            // Thông tin cán bộ
            var canBo = await _context.CanBos
                .Include(x => x.DonVi)
                .Include(x => x.ChucVu)
                .FirstOrDefaultAsync(x => x.MaCB == MaCB);

            if (canBo == null)
            {
                return BaseResult.Fail("Không tìm thấy cán bộ");
            }

            if (canBo.DonVi == null || canBo.ChucVu == null)
            {
                return BaseResult.Fail("Thông tin cán bộ không hợp lệ");
            }

            // Trưởng phòng CTSV xác nhận doanh nghiệp trước khi khoa xác nhận hợp tác
            if (trangThai == HopTacDoanhNghiepStatus.XacNhanDoanhNghiep)
            {
                // Bắt buộc vừa thuộc PCTSV vừa là Trưởng phòng
                if (canBo.DonVi.TenDV != "Phòng Công Tác Sinh Viên" ||
                    canBo.ChucVu.TenChucVu != "Trưởng Phòng")
                {
                    return BaseResult.Fail("Bạn không có quyền xác nhận doanh nghiệp");
                }

                if (doanhNghiep.TrangThaiHopTac ==
                    HopTacDoanhNghiepStatus.XacNhanDoanhNghiep)
                {
                    return BaseResult.Fail("Doanh nghiệp đã được xác nhận");
                }

                doanhNghiep.TrangThaiHopTac =
                    HopTacDoanhNghiepStatus.XacNhanDoanhNghiep;
            }

            // Cập nhật xác nhận hợp tác sẽ xử lý riêng ở các khoa
            if (trangThai == HopTacDoanhNghiepStatus.XacNhanHopTac)
            {
                return BaseResult.Fail("Trạng thái không hợp lệ");
            }

            // Ban giám hiệu duyệt hợp tác sau doanh nghiệp được ít nhất một trưởng khoa xác nhận hợp tác
            if (trangThai == HopTacDoanhNghiepStatus.DuyetHopTac)
            {
                // Bắt buộc vừa thuộc BGH vừa là Hiệu Trưởng
                if (canBo.DonVi.TenDV != "Ban Giám Hiệu" ||
                    canBo.ChucVu.TenChucVu != "Hiệu Trưởng")
                {
                    return BaseResult.Fail("Bạn không có quyền duyệt hợp tác");
                }
                if (doanhNghiep.TrangThaiHopTac ==
                    HopTacDoanhNghiepStatus.DuyetHopTac)
                {
                    return BaseResult.Fail("Doanh nghiệp đã được duyệt");
                }
                doanhNghiep.TrangThaiHopTac =
                    HopTacDoanhNghiepStatus.DuyetHopTac;
            }

            if (trangThai == HopTacDoanhNghiepStatus.TuChoi)
            {
                if (doanhNghiep.TrangThaiHopTac == HopTacDoanhNghiepStatus.TuChoi)
                {
                    return BaseResult.Fail("Doanh nghiệp đã bị từ chối");
                }

                var canReject = canBo.DonVi.TenDV == "Phòng Công Tác Sinh Viên" && canBo.ChucVu.TenChucVu == "Trưởng Phòng" ||
                                canBo.DonVi.TenDV == "Ban Giám Hiệu" && canBo.ChucVu.TenChucVu == "Hiệu Trưởng";
                if (!canReject)
                {
                    return BaseResult.Fail("Bạn không có quyền từ chối");
                }

                doanhNghiep.TrangThaiHopTac = HopTacDoanhNghiepStatus.TuChoi;
            }

            doanhNghiep.UpdatedAt = DateTime.UtcNow;
            doanhNghiep.UpdatedBy = MaCB;

            try
            {
                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    return BaseResult.Fail("Cập nhật trạng thái hợp tác thất bại");
                }

                return BaseResult.Success("Cập nhật trạng thái hợp tác thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Đã có lỗi xảy ra: " + ex.Message);
            }
        }

        public async Task<PageResult<HopTacDonViVM>> GetListHopTacDonVi(int pageIndex, int pageSize, string? keyword, string MaCB)
        {
            var result = new PageResult<HopTacDonViVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = 0,
                Records = new List<HopTacDonViVM>()
            };

            var canBo = await _context.CanBos.Include(x => x.DonVi).AsNoTracking().FirstOrDefaultAsync(x => x.MaCB == MaCB);

            if (canBo == null) { 
                return result;
            }

            var query = _context.HopTacDonVis
                .Include(x => x.DoanhNghiep)
                .Include(x => x.DonVi)
                .Where(x => x.MaDV == canBo.DonVi.MaDV && x.DoanhNghiep.TrangThaiHopTac != HopTacDoanhNghiepStatus.ChoXuLy && x.DoanhNghiep.TrangThaiHopTac != HopTacDoanhNghiepStatus.TuChoi)
                .AsNoTracking();
                
            if (!string.IsNullOrEmpty(keyword))
            {
                var normalizedKeyword = keyword.Trim();
                query = query.Where(x => x.DoanhNghiep.TenHienThi.Contains(normalizedKeyword) || x.DoanhNghiep.MaDN.Contains(normalizedKeyword) || (x.DoanhNghiep.MaSoThue != null && x.DoanhNghiep.MaSoThue.Contains(normalizedKeyword)));
            }

            var totalRecords = await query.CountAsync();

            var records = await query.OrderByDescending(x => x.CreatedAt)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .Select(x => new HopTacDonViVM
                                     {
                                         MaHTDV = x.MaHTDV,
                                         MaDN = x.MaDN,
                                         TenHienThi = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                                         TenPhapLy = x.DoanhNghiep != null ? x.DoanhNghiep.TenPhapLy : null,
                                         MaSoThue = x.DoanhNghiep != null ? x.DoanhNghiep.MaSoThue : null,
                                         Website = x.DoanhNghiep != null ? x.DoanhNghiep.Website : null,
                                         Hotline = x.DoanhNghiep != null ? x.DoanhNghiep.Hotline : null,
                                         EmailCongTy = x.DoanhNghiep != null ? x.DoanhNghiep.EmailCongTy : null,
                                         NoiDungHopTac = x.DoanhNghiep != null ? x.DoanhNghiep.NoiDungHopTac : null,
                                         MaDV = x.MaDV,
                                         TenDV = x.DonVi != null ? x.DonVi.TenDV : null,
                                         DonViTel = x.DonVi != null ? x.DonVi.Tel : null,
                                         DonViEmail = x.DonVi != null ? x.DonVi.Email : null,
                                         DonViWebsite = x.DonVi != null ? x.DonVi.Website : null,
                                         TrangThaiHopTac = x.TrangThai,
                                         CreatedAt = x.CreatedAt,
                                         UpdatedAt = x.UpdatedAt
                                     }).ToListAsync();

            result.TotalRecords = totalRecords;
            result.Records = records;

            return result;
        }

        // Hàm dùng cho khoa
        public async Task<BaseResult> UpdateTrangThaiHopTacDV(int maHTDV, HopTacDonViStatus trangThai, string MaCB)
        {
            if (maHTDV <= 0 || string.IsNullOrEmpty(MaCB) || !Enum.IsDefined(typeof(HopTacDonViStatus), trangThai))
            {
                return BaseResult.Fail("Dữ liệu không hợp lệ");
            }

            var hopTacDonVi = await _context.HopTacDonVis.FirstOrDefaultAsync(x => x.MaHTDV == maHTDV);

            if (hopTacDonVi == null)
            {
                return BaseResult.Fail("Không tìm thấy hợp tác đơn vị");
            }

            if (hopTacDonVi.TrangThai != HopTacDonViStatus.ChoPhanHoi)
            {
                return BaseResult.Fail("Trạng thái hiện tại không thể cập nhật");
            }

            var canBo = await _context.CanBos.Include(x => x.ChucVu).FirstOrDefaultAsync(x => x.MaCB == MaCB);

            if (canBo == null || canBo.ChucVu == null)
            {
                return BaseResult.Fail("Không tìm thấy thông tin cán bộ");
            }

            if (canBo.ChucVu.TenChucVu != "Trưởng Khoa")
            {
                return BaseResult.Fail("Bạn không có quyền cập nhật trạng thái hợp tác đơn vị");
            }

            var doanhNghiep = await _context.DoanhNghieps.FirstOrDefaultAsync(x => x.MaDN == hopTacDonVi.MaDN);

            if (doanhNghiep == null)
            {
                return BaseResult.Fail("Không tìm thấy doanh nghiệp liên quan");
            }

            if (doanhNghiep.TrangThaiHopTac == HopTacDoanhNghiepStatus.XacNhanDoanhNghiep && trangThai == HopTacDonViStatus.HopTac)
            {
                doanhNghiep.TrangThaiHopTac = HopTacDoanhNghiepStatus.XacNhanHopTac;
            }

            hopTacDonVi.TrangThai = trangThai;
            hopTacDonVi.UpdatedAt = DateTime.UtcNow;
            hopTacDonVi.UpdatedBy = MaCB;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BaseResult.Fail("Cập nhật trạng thái thất bại");
            }

            return BaseResult.Success("Cập nhật trạng thái thành công");
        }

        public async Task<BaseResult<HopTacDonViVM>> GetHopTacDonViByMaHTDV(int MaHTDV)
        {
            if (MaHTDV <= 0) { 
                return BaseResult<HopTacDonViVM>.Fail("Mã hợp tác đơn vị không hợp lệ");
            }

            var item = await _context.HopTacDonVis
                                     .Include(x => x.DoanhNghiep)
                                     .Include(x => x.DonVi)
                                     .AsNoTracking()
                                     .Select(x => new HopTacDonViVM
                                     {
                                         MaHTDV = x.MaHTDV,
                                         MaDN = x.MaDN,
                                         TenHienThi = x.DoanhNghiep != null ? x.DoanhNghiep.TenHienThi : null,
                                         TenPhapLy = x.DoanhNghiep != null ? x.DoanhNghiep.TenPhapLy : null,
                                         MaSoThue = x.DoanhNghiep != null ? x.DoanhNghiep.MaSoThue : null,
                                         Website = x.DoanhNghiep != null ? x.DoanhNghiep.Website : null,
                                         Hotline = x.DoanhNghiep != null ? x.DoanhNghiep.Hotline : null,
                                         EmailCongTy = x.DoanhNghiep != null ? x.DoanhNghiep.EmailCongTy : null,
                                         NoiDungHopTac = x.DoanhNghiep != null ? x.DoanhNghiep.NoiDungHopTac : null,
                                         MaDV = x.MaDV,
                                         TenDV = x.DonVi != null ? x.DonVi.TenDV : null,
                                         DonViTel = x.DonVi != null ? x.DonVi.Tel : null,
                                         DonViEmail = x.DonVi != null ? x.DonVi.Email : null,
                                         DonViWebsite = x.DonVi != null ? x.DonVi.Website : null,
                                         TrangThaiHopTac = x.TrangThai,
                                         CreatedAt = x.CreatedAt,
                                         UpdatedAt = x.UpdatedAt
                                     })
                                     .FirstOrDefaultAsync(x => x.MaHTDV == MaHTDV);

            if (item == null)
            {
                return BaseResult<HopTacDonViVM>.Fail("Không tìm thấy hợp tác đơn vị");
            }

            return BaseResult<HopTacDonViVM>.Success(item);
        }

        public async Task<BaseResult<NguoiDaiDienVM>> GetNguoiDaiDienInfo(string MaDoanhNghiep)
        {
            if (string.IsNullOrWhiteSpace(MaDoanhNghiep))
            {
                return BaseResult<NguoiDaiDienVM>.Fail("Mã doanh nghiệp không hợp lệ");
            }

            var dn = await _context.DoanhNghieps
                .Include(x => x.NguoiDung)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaDN == MaDoanhNghiep && x.DeletedAt == null);

            if (dn == null)
            {
                return BaseResult<NguoiDaiDienVM>.Fail("Không tìm thấy doanh nghiệp");
            }

            var user = dn.NguoiDung;

            var vm = new NguoiDaiDienVM
            {
                HoTen = user?.HoTen,
                SoDienThoai = user?.PhoneNumber,
                Email = user?.Email,
                AnhNguoiDaiDien = user?.AnhDaiDien
            };

            return BaseResult<NguoiDaiDienVM>.Success(vm, "Lấy thông tin người đại diện thành công");
        }
        public async Task<BaseResult> UpdateNguoiDaiDienInfo(string MaDoanhNghiep, NguoiDaiDienUpdateVM updateVM)
        {
            if (string.IsNullOrWhiteSpace(MaDoanhNghiep))
            {
                return BaseResult.Fail("Mã doanh nghiệp không hợp lệ");
            }

            if (updateVM == null)
            {
                return BaseResult.Fail("Dữ liệu cập nhật không hợp lệ");
            }

            var doanhNghiep = await _context.DoanhNghieps
                .Include(x => x.NguoiDung)
                .FirstOrDefaultAsync(x => x.MaDN == MaDoanhNghiep && x.DeletedAt == null);

            if (doanhNghiep == null)
            {
                return BaseResult.Fail("Không tìm thấy doanh nghiệp");
            }

            var user = doanhNghiep.NguoiDung;
            if (user == null)
            {
                return BaseResult.Fail("Người đại diện chưa được gán");
            }

            user.HoTen = updateVM.HoTen.Trim();
            user.PhoneNumber = updateVM.SoDienThoai.Trim();
            user.Email = updateVM.Email.Trim();
            user.AnhDaiDien = string.IsNullOrWhiteSpace(updateVM.AnhNguoiDaiDien) ? user.AnhDaiDien : updateVM.AnhNguoiDaiDien.Trim();

            await _context.SaveChangesAsync();

            return BaseResult.Success("Cập nhật người đại diện thành công");
        }
    }
}
