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
                CreatedAt = DateTime.UtcNow,
                CreatedBy = danhMuc.CreatedBy
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

        public async Task<BaseResult> EditDanhMucBaiViet(int maDanhMuc, DanhMucBaiVietEditVM danhMuc)
        {
            var item = await _context.DanhMucBaiViets.FirstOrDefaultAsync(x => x.MaDanhMuc == maDanhMuc && x.DeletedAt == null);

            if(item == null)
            {
                return BaseResult.Fail("Danh mục bài viết không tồn tại.");
            }

            if (!string.Equals(item.Ten, danhMuc.Ten, StringComparison.Ordinal))
            {
                var slug = await _slug.GenerateUniqueSlugAsync(
                    danhMuc.Ten,
                    _context.DanhMucBaiViets.AsNoTracking().Where(x => x.MaDanhMuc != maDanhMuc && x.DeletedAt == null),
                    x => x.Slug
                );

                item.Ten = danhMuc.Ten;
                item.Slug = slug;
            }

            item.MoTa = danhMuc.MoTa;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = danhMuc.UpdatedBy;

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

        public async Task<BaseResult> DeleteDanhMucBaiViet(int maDanhMuc, string deletedBy)
        {
            var item = await _context.DanhMucBaiViets.FirstOrDefaultAsync(x=> x.MaDanhMuc == maDanhMuc && x.DeletedAt == null);

            if (item == null)
            {
                return BaseResult.Fail("Danh mục bài viết không tồn tại.");
            }

            var hasBaiViet = await _context.BaiViets.AnyAsync(x => x.MaDanhMuc == maDanhMuc && x.DeletedAt == null);

            if (hasBaiViet)
            {
                return BaseResult.Fail("Không thể xóa danh mục bài viết vì có bài viết liên quan.");
            }

            try
            {
                item.DeletedAt = DateTime.UtcNow;
                item.DeletedBy = deletedBy;
                await _context.SaveChangesAsync();
                return BaseResult.Success("Xóa danh mục bài viết thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Đã có lỗi xảy ra khi xóa danh mục bài viết.");
            }          
        }

        public async Task<BaseResult<DanhMucBaiVietVM>> GetDanhMucBaiVietById(int maDanhMuc)
        {
            var result = await _context.DanhMucBaiViets
                .AsNoTracking()
                .Where(x => x.MaDanhMuc == maDanhMuc && x.DeletedAt == null) 
                .Select(x => new DanhMucBaiVietVM
                {
                    MaDanhMuc = x.MaDanhMuc,
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
            var query = _context.DanhMucBaiViets.AsNoTracking().Where(x => x.DeletedAt == null); 

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
                    MaDanhMuc = x.MaDanhMuc,
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
