using HopTacDoanhNghiep.Areas.Admin.ViewModels.DoanhNghiep;
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

        public async Task<BaseResult<DoanhNghiepVM>> GetDoanhNghiepByMaDN(string maDN)
        {
            if (string.IsNullOrWhiteSpace(maDN))
            {
                return BaseResult<DoanhNghiepVM>.Fail("Mã doanh nghiệp không được để trống.");
            }

            maDN = maDN.Trim();

            var doanhNghiep = await _context.DoanhNghieps
                .AsNoTracking()
                .Where(dn => dn.DeletedAt == null && dn.MaDN == maDN)
                .Select(dn => new DoanhNghiepVM
                {
                    MaDN = dn.MaDN,
                    TenHienThi = dn.TenHienThi,
                    Website = dn.Website,
                    MaSoThue = dn.MaSoThue,
                    TenPhapLy = dn.TenPhapLy,
                    Hotline = dn.Hotline,
                    EmailCongTy = dn.EmailCongTy,
                    Logo = dn.Logo,
                    DiaChi = dn.DiaChi,
                    GioiThieu = dn.GioiThieu,
                    QuyMoNhanSu = dn.QuyMoNhanSu,
                    TrangThaiHopTac = dn.TrangThaiHopTac
                })
                .FirstOrDefaultAsync();

            if (doanhNghiep == null)
            {
                return BaseResult<DoanhNghiepVM>.Fail("Doanh nghiệp không tồn tại.");
            }

            return BaseResult<DoanhNghiepVM>.Success(doanhNghiep);
        }

        public async Task<PageResult<DoanhNghiepVM>> GetListDoanhNghiep(int pageIndex, int pageSize, string? keyword)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var query = _context.DoanhNghieps.AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(dn => dn.TenHienThi.Contains(keyword) || dn.MaDN.Contains(keyword) || dn.TenPhapLy.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            var doanhNghieps = await query
                .OrderBy(dn => dn.TenHienThi)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(dn => new DoanhNghiepVM
                {
                    MaDN = dn.MaDN,
                    TenHienThi = dn.TenHienThi,
                    Website = dn.Website,
                    MaSoThue = dn.MaSoThue,
                    TenPhapLy = dn.TenPhapLy,
                    Hotline = dn.Hotline,
                    EmailCongTy = dn.EmailCongTy,
                    Logo = dn.Logo,
                    DiaChi = dn.DiaChi,
                    GioiThieu = dn.GioiThieu,
                    QuyMoNhanSu = dn.QuyMoNhanSu,
                    TrangThaiHopTac = dn.TrangThaiHopTac
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
