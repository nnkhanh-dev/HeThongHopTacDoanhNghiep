using HopTacDoanhNghiep.Areas.Admin.ViewModels.DonVi;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class DonViAdminService : IDonViAdmin
    {
        private readonly AppDbContext _context;

        public DonViAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<DonViVM>> GetDonViAsync(int pageIndex, int pageSize, string? keyword)
        {
            var query = _context.DonVis.AsNoTracking().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TenDV.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var records = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DonViVM
                {
                    MaDonVi = x.MaDV,
                    TenDonVi = x.TenDV
                })
                .ToListAsync();

            return new PageResult<DonViVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            };
        }
    }
}
