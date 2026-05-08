using HopTacDoanhNghiep.Areas.Admin.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class DoanhNghiepAdminService : IDoanhNghiepAdmin
    {
        private readonly AppDbContext _context;

        public DoanhNghiepAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepById(Guid id)
        {
            var doanhNghiep = await _context.DoanhNghieps.AsNoTracking()
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new DoanhNghiepVM
                {
                    Id = x.Id,
                    MaDN = x.MaDN,
                    TenHienThi = x.TenHienThi,
                    Website = x.Website,
                    MaSoThue = x.MaSoThue,
                    NgayThanhLap = x.NgayThanhLap,
                    TenPhapLy = x.TenPhapLy,
                    SDT = x.SDT,
                    Email = x.Email,
                    Logo = x.Logo,
                    DiaChi = x.DiaChi,
                    GioiThieu = x.GioiThieu,
                    QuyMoNhanSu = x.QuyMoNhanSu,
                    GhiChu = x.GhiChu
                })
                .FirstOrDefaultAsync();

            if (doanhNghiep == null)
            {
                return BaseResult<DoanhNghiepVM>.Fail("Doanh nghiệp không tồn tại");
            }

            return BaseResult<DoanhNghiepVM>.Success(doanhNghiep, "Lấy thông tin doanh nghiệp thành công!");
        }

        public async Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var query = _context.DoanhNghieps.Where(x => x.DeletedAt == null).AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TenHienThi.Contains(keyword) || x.MaSoThue.Contains(keyword) || x.MaDN.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            var doanhNghieps = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DoanhNghiepVM
                {
                    Id = x.Id,
                    MaDN = x.MaDN,
                    TenHienThi = x.TenHienThi,
                    MaSoThue = x.MaSoThue,
                    Website = x.Website,
                    DiaChi = x.DiaChi
                })
                .ToListAsync();

            return new PageResult<DoanhNghiepVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = doanhNghieps
            };
        }
    }
}
