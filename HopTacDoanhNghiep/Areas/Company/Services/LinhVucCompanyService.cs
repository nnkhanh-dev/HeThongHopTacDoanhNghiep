using HopTacDoanhNghiep.Areas.Company.ViewModels.LinhVuc;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public class LinhVucCompanyService : ILinhVucCompany
    {
        private readonly AppDbContext _context;

        public LinhVucCompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<LinhVucVM>> GetListLinhVuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var query = _context.LinhVucs.AsNoTracking().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Ten.Contains(keyword) || x.MoTa.Contains(keyword));
            }

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalRecords = await query.CountAsync();

            var data = await query.OrderByDescending(x => x.CreatedAt)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .Select(x => new LinhVucVM
                                  {
                                      Id = x.Id,
                                      Ten = x.Ten,
                                      MoTa = x.MoTa,
                                      Slug = x.Slug
                                  })
                                  .ToListAsync();

            return new PageResult<LinhVucVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = data
            };
        }
    }
}
