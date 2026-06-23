using HopTacDoanhNghiep.Areas.Admin.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class DonUngTuyenAdminService : IDonUngTuyenAdmin
    {
        private readonly AppDbContext _context;

        public DonUngTuyenAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<DonUngTuyenVM>> GetDonUngTuyenById(int MaTTD)
        {
            var item = await _context.DonUngTuyens
                .AsNoTracking()
                .Where(x => x.MaTTD == MaTTD
                    && x.DeletedAt == null
                    && x.TinTuyenDung != null
                   )
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

        public async Task<PageResult<DonUngTuyenVM>> GetListDonUngTuyen(int pageInge, int pageSize, string? keyword, HoSoStatus? hoSoStatus, int? maTTD)
        {
            if (pageInge < 1) pageInge = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.DonUngTuyens
                .AsNoTracking()
                .Where(x => x.DeletedAt == null
                    && x.TinTuyenDung != null
                    && x.TrangThai != HoSoStatus.RutHoSo);

            if (hoSoStatus.HasValue)
            {
                query = query.Where(x => x.TrangThai == hoSoStatus.Value);
            }

            if (maTTD.HasValue)
            {
                query = query.Where(x => x.MaTTD == maTTD);
            }

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

        
    }
}
