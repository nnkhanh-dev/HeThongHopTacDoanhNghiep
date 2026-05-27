using HopTacDoanhNghiep.Areas.Admin.ViewModels.ChucVu;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class ChucVuAdminService : IChucVuAdmin
    {
        private readonly AppDbContext _context;
        public ChucVuAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<ChucVuVM>> GetChucVuAsync(int pageIndex, int pageSize, string? keyword)
        {
            var query = _context.ChucVus.AsNoTracking().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TenChucVu.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var records = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ChucVuVM
                {
                    maChucVu = x.MaChucVu,
                    tenChucVu = x.TenChucVu
                })
                .ToListAsync();

            return new PageResult<ChucVuVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            };
        }
    }
}
