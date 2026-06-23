using HopTacDoanhNghiep.Areas.Student.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.Areas.Student.ViewModels.ViecLam;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Student.Services
{
    public class DonUngTuyenStudentService : IDonUngTuyenStudent
    {
        private readonly AppDbContext _context;

        public DonUngTuyenStudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult> ApplyDonUngTuyen(DonUngTuyenCreateVM donUngTuyen)
        {
            if(donUngTuyen == null)
            {
                return BaseResult.Fail("Dữ liệu ứng tuyển không hợp lệ");
            }

            if(donUngTuyen.MaTTD <= 0)
            {
                return BaseResult.Fail("Tin tuyển dụng không hợp lệ");
            }

            if(string.IsNullOrEmpty(donUngTuyen.MaSV))
            {
                return BaseResult.Fail("Sinh viên không hợp lệ");
            }

            if(string.IsNullOrWhiteSpace(donUngTuyen.MaSV))
            {
                return BaseResult.Fail("Sinh viên không hợp lệ");
            }

            if(string.IsNullOrWhiteSpace(donUngTuyen.HoSoUngTuyen))
            {
                return BaseResult.Fail("Vui lòng tải lên hồ sơ ứng tuyển");
            }

            var isValidStudent = await _context.SinhViens.AnyAsync(x => x.MaSV == donUngTuyen.MaSV && x.DeletedAt == null);

            if(!isValidStudent) {
                return BaseResult.Fail("Sinh viên không hợp lệ");
            }

            var isValidTinTuyenDung = await _context.TinTuyenDungs.AnyAsync(x => x.MaTTD == donUngTuyen.MaTTD && x.DeletedAt == null);

            if(!isValidTinTuyenDung) {
                return BaseResult.Fail("Tin tuyển dụng không hợp lệ");
            }

            var isApplied = await _context.DonUngTuyens.AnyAsync(x => x.MaSV == donUngTuyen.MaSV && x.MaTTD == donUngTuyen.MaTTD && x.DeletedAt == null);

            if(isApplied) {
                return BaseResult.Fail("Bạn đã ứng tuyển vào tin tuyển dụng này");
            }

            var data = new DonUngTuyen
            {
                MaTTD = donUngTuyen.MaTTD,
                MaSV = donUngTuyen.MaSV,
                HoSoUngTuyen = donUngTuyen.HoSoUngTuyen,
                TrangThai = HoSoStatus.ChoPhanHoi,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = donUngTuyen.MaSV
            };

            await _context.DonUngTuyens.AddAsync(data);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                return BaseResult.Fail("Ứng tuyển thất bại");
            }

            return BaseResult.Success("Ứng tuyển thành công");
        }

        public async Task<BaseResult<DonUngTuyenVM>> GetDonUngTuyenById(int MaTTD, string MaSinhVien)
        {
            var item = await _context.DonUngTuyens
                .AsNoTracking()
                .Include(x => x.SinhVien)
                    .ThenInclude(x => x.NguoiDung)
                .Include(x => x.TinTuyenDung)
                .Where(x => x.MaTTD == MaTTD && x.MaSV == MaSinhVien && x.DeletedAt == null)
                .Select(x => new DonUngTuyenVM
                {
                    MaUT = x.MaUT,
                    MaSV = x.MaSV,
                    TenSinhVien = x.SinhVien != null && x.SinhVien.NguoiDung != null ? x.SinhVien.NguoiDung.HoTen : null,
                    MaTTD = x.MaTTD,
                    TinTuyenDung = x.TinTuyenDung != null ? new TinTuyenDungVM
                    {
                        MaTTD = x.TinTuyenDung.MaTTD,
                        TieuDe = x.TinTuyenDung.TieuDe,
                        Slug = x.TinTuyenDung.Slug,
                        MoTa = x.TinTuyenDung.MoTa,
                        YeuCau = x.TinTuyenDung.YeuCau,
                        UuTien = x.TinTuyenDung.UuTien,
                        QuyenLoi = x.TinTuyenDung.QuyenLoi,
                        LuongToiThieu = x.TinTuyenDung.LuongToiThieu,
                        LuongToiDa = x.TinTuyenDung.LuongToiDa,
                        DiaDiem = x.TinTuyenDung.DiaDiem,
                        TuKhoa = x.TinTuyenDung.TuKhoa,
                        NgayBatDau = x.TinTuyenDung.NgayBatDau,
                        NgayHetHan = x.TinTuyenDung.NgayHetHan,
                        LoaiViecLam = x.TinTuyenDung.LoaiViecLam,
                        DoiTuongUngTuyen = x.TinTuyenDung.DoiTuongUngTuyen,
                        TrinhDo = x.TinTuyenDung.TrinhDo,
                        Status = x.TinTuyenDung.Status,
                        MaDoanhNghiep = x.TinTuyenDung.MaDoanhNgiep,
                        DoanhNghiep = x.TinTuyenDung.DoanhNghiep != null ? x.TinTuyenDung.DoanhNghiep.TenHienThi : null,
                        LogoDoanhNghiep = x.TinTuyenDung.DoanhNghiep != null ? x.TinTuyenDung.DoanhNghiep.Logo : null,
                        CreatedAt = x.TinTuyenDung.CreatedAt
                    } : null,
                    HoSoUngTuyen = x.HoSoUngTuyen,
                    TrangThai = x.TrangThai,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return BaseResult<DonUngTuyenVM>.Fail("Đơn ứng tuyển không tồn tại");
            }

            return BaseResult<DonUngTuyenVM>.Success(item, "Lấy dữ liệu đơn ứng tuyển thành công");
        }

        public async Task<PageResult<DonUngTuyenVM>> GetListDonUngTuyen(int pageInge, int pageSize, string? keyword, string MaSinhVien, HoSoStatus? trangthai)
        {
            if (pageInge < 1) pageInge = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.DonUngTuyens
                .AsNoTracking()
                .Where(x => x.MaSV == MaSinhVien && x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.Trim();
                query = query.Where(x =>
                    (x.SinhVien != null && x.SinhVien.NguoiDung != null && EF.Functions.Like(x.SinhVien.NguoiDung.HoTen, "%" + k + "%"))
                    || (x.TinTuyenDung != null && (
                        EF.Functions.Like(x.TinTuyenDung.TieuDe, "%" + k + "%")
                        || EF.Functions.Like(x.TinTuyenDung.MoTa, "%" + k + "%")
                        || EF.Functions.Like(x.TinTuyenDung.TuKhoa, "%" + k + "%")
                    )));
            }

            if (trangthai.HasValue)
            {
                query = query.Where(x => x.TrangThai == trangthai.Value);
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageInge - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DonUngTuyenVM
                {
                    MaUT = x.MaUT,
                    MaSV = x.MaSV,
                    TenSinhVien = x.SinhVien != null && x.SinhVien.NguoiDung != null ? x.SinhVien.NguoiDung.HoTen : null,
                    MaTTD = x.MaTTD,
                    TinTuyenDung = x.TinTuyenDung != null ? new TinTuyenDungVM
                    {
                        MaTTD = x.TinTuyenDung.MaTTD,
                        TieuDe = x.TinTuyenDung.TieuDe,
                        Slug = x.TinTuyenDung.Slug,
                        MoTa = x.TinTuyenDung.MoTa,
                        YeuCau = x.TinTuyenDung.YeuCau,
                        UuTien = x.TinTuyenDung.UuTien,
                        QuyenLoi = x.TinTuyenDung.QuyenLoi,
                        LuongToiThieu = x.TinTuyenDung.LuongToiThieu,
                        LuongToiDa = x.TinTuyenDung.LuongToiDa,
                        DiaDiem = x.TinTuyenDung.DiaDiem,
                        TuKhoa = x.TinTuyenDung.TuKhoa,
                        NgayBatDau = x.TinTuyenDung.NgayBatDau,
                        NgayHetHan = x.TinTuyenDung.NgayHetHan,
                        LoaiViecLam = x.TinTuyenDung.LoaiViecLam,
                        DoiTuongUngTuyen = x.TinTuyenDung.DoiTuongUngTuyen,
                        TrinhDo = x.TinTuyenDung.TrinhDo,
                        Status = x.TinTuyenDung.Status,
                        MaDoanhNghiep = x.TinTuyenDung.MaDoanhNgiep,
                        DoanhNghiep = x.TinTuyenDung.DoanhNghiep != null ? x.TinTuyenDung.DoanhNghiep.TenHienThi : null,
                        LogoDoanhNghiep = x.TinTuyenDung.DoanhNghiep != null ? x.TinTuyenDung.DoanhNghiep.Logo : null,
                        CreatedAt = x.TinTuyenDung.CreatedAt
                    } : null,
                    HoSoUngTuyen = x.HoSoUngTuyen,
                    TrangThai = x.TrangThai,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return new PageResult<DonUngTuyenVM>
            {
                PageIndex = pageInge,
                PageSize = pageSize,
                TotalRecords = total,
                Records = items
            };
        }

        public async Task<BaseResult> WithdrawApplication(int MaUT, string MaSinhVien)
        {
            // Find the application belonging to the student
            var entity = await _context.DonUngTuyens
                .Where(x => x.MaUT == MaUT && x.MaSV == MaSinhVien && x.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return BaseResult.Fail("Đơn ứng tuyển không tồn tại hoặc không thuộc sinh viên này");
            }

            // Only allow withdrawal when the application is still pending (ChoPhanHoi)
            if (entity.TrangThai != HoSoStatus.ChoPhanHoi)
            {
                return BaseResult.Fail("Chỉ có thể rút hồ sơ khi trạng thái đang ở 'Chờ phản hồi'.");
            }

            try
            {
                entity.TrangThai = HoSoStatus.RutHoSo;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = MaSinhVien;

                _context.DonUngTuyens.Update(entity);
                var saved = await _context.SaveChangesAsync() > 0;
                if (!saved)
                    return BaseResult.Fail("Cập nhật trạng thái thất bại");

                return BaseResult.Success("Rút hồ sơ thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Lỗi khi rút hồ sơ: " + ex.Message);
            }
        }
    }
}
