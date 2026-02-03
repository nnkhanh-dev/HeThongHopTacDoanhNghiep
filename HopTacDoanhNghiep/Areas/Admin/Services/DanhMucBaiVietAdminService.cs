using HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class DanhMucBaiVietAdminService : IDanhMucBaiVietAdmin
    {
        private readonly AppDbContext _context;
        private readonly ISlug _slug;

        public DanhMucBaiVietAdminService(AppDbContext context, ISlug slug)
        {
            _context = context;
            _slug = slug;
        }


        public async Task<BaseResult> CreateDanhMucBaiViet(DanhMucBaiVietCreateVM danhMuc)
        {
            var slug = await _slug.GenerateUniqueSlugAsync(
                danhMuc.Ten,
                _context.DanhMucBaiViets.AsNoTracking(),
                x => x.Slug
            );

            var item = new DanhMucBaiViet
            {
                Ten = danhMuc.Ten,
                MoTa = danhMuc.MoTa,
                Slug = slug,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.DanhMucBaiViets.Add(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Tạo danh mục bài viết thành công");
            }
            catch (DbUpdateException)
            {
                return BaseResult.Fail("Slug bị trùng, vui lòng thử lại");
            }
        }

        public async Task<BaseResult> EditDanhMucBaiViet(int id, DanhMucBaiVietEditVM danhMuc)
        {
            var item = await _context.DanhMucBaiViets.FindAsync(id);

            if(item == null)
            {
                return BaseResult.Fail("Danh mục bài viết không tồn tại.");
            }

            if (!string.Equals(item.Ten, danhMuc.Ten, StringComparison.Ordinal))
            {
                var slug = await _slug.GenerateUniqueSlugAsync(
                    danhMuc.Ten,
                    _context.DanhMucBaiViets.AsNoTracking().Where(x => x.Id != id),
                    x => x.Slug
                );

                item.Ten = danhMuc.Ten;
                item.Slug = slug;
            }

            item.MoTa = danhMuc.MoTa;
            item.UpdatedAt = DateTime.Now;

            try
            {
                _context.DanhMucBaiViets.Update(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Chỉnh sửa danh mục bài viết thành công");
            }
            catch (DbUpdateException)
            {
                return BaseResult.Fail("Slug bị trùng, vui lòng thử lại");
            }
        }

        public async Task<BaseResult> DeleteDanhMucBaiViet(int id)
        {
            var item = await _context.DanhMucBaiViets.FindAsync(id);

            if (item == null)
            {
                return BaseResult.Fail("Danh mục bài viết không tồn tại.");
            }

            var hasBaiViet = await _context.BaiViets.AnyAsync(x => x.DanhMucId == id);

            if (hasBaiViet)
            {
                return BaseResult.Fail("Không thể xóa danh mục bài viết vì có bài viết liên quan.");
            }

            _context.DanhMucBaiViets.Remove(item);
            await _context.SaveChangesAsync();
            return BaseResult.Success("Xóa danh mục bài viết thành công");
        }

        public async Task<BaseResult<DanhMucBaiVietVM>> GetDanhMucBaiVietById(int id)
        {
            var result = await _context.DanhMucBaiViets
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new DanhMucBaiVietVM
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MoTa = x.MoTa,
                    Slug = x.Slug,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return BaseResult<DanhMucBaiVietVM>.Fail("Danh mục bài viết không tồn tại.");
            }

            return BaseResult<DanhMucBaiVietVM>.Success(result, "Lấy dữ liệu danh mục thành công.");
        }

        public async Task<PageResult<DanhMucBaiVietVM>> GetListDanhMucBaiViet(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var query = _context.DanhMucBaiViets.AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Ten.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var records = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DanhMucBaiVietVM
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MoTa = x.MoTa,
                    Slug = x.Slug,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return new PageResult<DanhMucBaiVietVM> 
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = records
            };
        }
    }
}
