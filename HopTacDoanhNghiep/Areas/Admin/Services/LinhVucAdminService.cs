using HopTacDoanhNghiep.Areas.Admin.ViewModels.LinhVuc;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class LinhVucAdminService : ILinhVucAdmin
    {
        private readonly AppDbContext _context;
        private readonly ISlug _slug;

        public LinhVucAdminService(AppDbContext context, ISlug slug)
        {
            _context = context;
            _slug = slug;
        }

        public async Task<BaseResult> CreateLinhVuc(LinhVucCreateVM model)
        {
            var slug = await _slug.GenerateUniqueSlugAsync(
               model.Ten,
               _context.LinhVucs.AsNoTracking(),
               x => x.Slug
           );

            var item = new LinhVuc
            {
                Ten = model.Ten,
                MoTa = model.MoTa,
                Slug = slug,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = model.CreatedBy
            };

            try
            {
                _context.LinhVucs.Add(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Tạo lĩnh vực thành công");
            }
            catch (Exception)
            {
                return BaseResult.Fail("Có lỗi xảy ra");
            }
        }

        public async Task<BaseResult> DeleteLinhVuc(int id, string deletedBy)
        {
            var item = await _context.LinhVucs.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (item == null)
            {
                return BaseResult.Fail("Lĩnh vực không tồn tại.");
            }

            var hasNganh = await _context.LinhVucNganhs.AnyAsync(x => x.LinhVucId == id && x.DeletedAt == null);

            if (hasNganh)
            {
                return BaseResult.Fail("Không thể xóa danh mục bài viết vì có ngành liên quan.");
            }

            var hasViecLam= await _context.ViecLams.AnyAsync(x => x.LinhVucId == id && x.DeletedAt == null);

            if (hasViecLam)
            {
                return BaseResult.Fail("Không thể xóa lĩnh vực vì có việc làm liên quan.");
            }

            try
            {
                item.DeletedAt = DateTime.UtcNow;
                item.DeletedBy = deletedBy;
                await _context.SaveChangesAsync();
                return BaseResult.Success("Xóa lĩnh vực thành công");
            }
            catch (Exception)
            {
                return BaseResult.Fail("Có lỗi xảy ra");
            }
        }

        public async Task<BaseResult> EditLinhVuc(int id, LinhVucEditVM model)
        {
            var item = await _context.LinhVucs.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (item == null)
            {
                return BaseResult.Fail("Lĩnh vực không tồn tại.");
            }

            if (!string.Equals(item.Ten, model.Ten, StringComparison.Ordinal))
            {
                var slug = await _slug.GenerateUniqueSlugAsync(
                    model.Ten,
                    _context.LinhVucs.AsNoTracking().Where(x => x.Id != id && x.DeletedAt == null),
                    x => x.Slug
                );

                item.Ten = model.Ten;
                item.Slug = slug;
            }

            item.MoTa = model.MoTa;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = model.UpdatedBy;

            try
            {
                _context.LinhVucs.Update(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Chỉnh sửa lĩnh vực thành công");
            }
            catch (Exception)
            {
                return BaseResult.Fail("Có lỗi xảy ra");
            }
        }

        public async Task<BaseResult<LinhVucVM>> GetLinhVucById(int id)
        {
            var result = await _context.LinhVucs
                .AsNoTracking()
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new LinhVucVM
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MoTa = x.MoTa,
                    Slug = x.Slug,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return BaseResult<LinhVucVM>.Fail("Lĩnh vực không tồn tại.");
            }

            return BaseResult<LinhVucVM>.Success(result, "Lấy dữ liệu lĩnh vực thành công.");
        }

        public async Task<PageResult<LinhVucVM>> GetListLinhVuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var query = _context.LinhVucs.AsNoTracking().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(lv => lv.Ten.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            if(pageIndex < 1) pageIndex = 1;
            if(pageSize <= 0) pageSize = 10;

            var records = await query
                .OrderByDescending(lv => lv.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new LinhVucVM
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MoTa = x.MoTa,
                    Slug = x.Slug,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return new PageResult<LinhVucVM>
            {
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Records = records
            };
        }
    }
}
