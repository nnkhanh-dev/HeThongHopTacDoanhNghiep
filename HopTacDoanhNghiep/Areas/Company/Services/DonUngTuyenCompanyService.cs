using HopTacDoanhNghiep.Areas.Company.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public class DonUngTuyenCompanyService : IDonUngTuyenCompany
    {
        private readonly AppDbContext _context;

        public DonUngTuyenCompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<DonUngTuyenVM>> GetDonUngTuyenById(int MaTTD, string MaDoanhNghiep)
        {
            var item = await _context.DonUngTuyens
                .AsNoTracking()
                .Where(x => x.MaTTD == MaTTD
                    && x.DeletedAt == null
                    && x.TinTuyenDung != null
                    && x.TinTuyenDung.MaDoanhNgiep == MaDoanhNghiep)
                .Select(x => new DonUngTuyenVM
                {
                    MaUT = x.MaUT,
                    MaSV = x.MaSV,
                    TenSinhVien = x.SinhVien != null && x.SinhVien.NguoiDung != null ? x.SinhVien.NguoiDung.HoTen : null,
                    MaTTD = x.MaTTD,
                    TieuDeTinTuyenDung = x.TinTuyenDung != null ? x.TinTuyenDung.TieuDe : null,
                    MaDoanhNghiep = x.TinTuyenDung != null ? x.TinTuyenDung.MaDoanhNgiep : null,
                    TenDoanhNghiep = x.TinTuyenDung != null && x.TinTuyenDung.DoanhNghiep != null ? x.TinTuyenDung.DoanhNghiep.TenHienThi : null,
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

        public async Task<PageResult<DonUngTuyenVM>> GetListDonUngTuyen(int pageInge, int pageSize, string? keyword, string MaDoanhNghiep)
        {
            if (pageInge < 1) pageInge = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.DonUngTuyens
                .AsNoTracking()
                .Where(x => x.DeletedAt == null
                    && x.TinTuyenDung != null
                    && x.TinTuyenDung.MaDoanhNgiep == MaDoanhNghiep);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.Trim();
                query = query.Where(x =>
                    (x.SinhVien != null
                        && x.SinhVien.NguoiDung != null
                        && EF.Functions.Like(x.SinhVien.NguoiDung.HoTen, "%" + k + "%"))
                    || (x.TinTuyenDung != null && (
                        EF.Functions.Like(x.TinTuyenDung.TieuDe, "%" + k + "%")
                        || EF.Functions.Like(x.TinTuyenDung.MoTa, "%" + k + "%")
                        || EF.Functions.Like(x.TinTuyenDung.TuKhoa, "%" + k + "%")
                    )));
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
                    TieuDeTinTuyenDung = x.TinTuyenDung != null ? x.TinTuyenDung.TieuDe : null,
                    MaDoanhNghiep = x.TinTuyenDung != null ? x.TinTuyenDung.MaDoanhNgiep : null,
                    TenDoanhNghiep = x.TinTuyenDung != null && x.TinTuyenDung.DoanhNghiep != null ? x.TinTuyenDung.DoanhNghiep.TenHienThi : null,
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

        public async Task<BaseResult> UpdateTrangThaiDonUngTuyen(int MaUT, HoSoStatus trangThai, string MaDoanhNghiep)
        {
            var entity = await _context.DonUngTuyens
                .Where(x => x.MaUT == MaUT && x.DeletedAt == null && x.TinTuyenDung != null && x.TinTuyenDung.MaDoanhNgiep == MaDoanhNghiep)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return BaseResult.Fail("Đơn ứng tuyển không tồn tại hoặc không thuộc doanh nghiệp");
            }

            // Validate allowed transitions: only allow from ChoPhanHoi -> ChapNhan or TuChoi
            if (entity.TrangThai != HoSoStatus.ChoPhanHoi)
            {
                return BaseResult.Fail("Chỉ có thể thay đổi trạng thái khi hồ sơ đang ở trạng thái 'Chờ phản hồi'.");
            }

            if (trangThai != HoSoStatus.ChapNhan && trangThai != HoSoStatus.TuChoi)
            {
                return BaseResult.Fail("Trạng thái đích không hợp lệ.");
            }

            try
            {
                entity.TrangThai = trangThai;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = MaDoanhNghiep;

                _context.DonUngTuyens.Update(entity);
                var saved = await _context.SaveChangesAsync() > 0;

                if (!saved)
                    return BaseResult.Fail("Cập nhật trạng thái thất bại");

                return BaseResult.Success("Cập nhật trạng thái thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Cập nhật trạng thái thất bại");
            }
        }
    }
}
