using HopTacDoanhNghiep.Areas.Admin.ViewModels.Nganh;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class NganhAdminService : INganhAdmin
    {
        private readonly AppDbContext _context;

        public NganhAdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult> CreateNganh(NganhCreateVM model)
        {
            var item = new Nganh
            {
                MaNganh = model.MaNganh,
                TenNganh = model.TenNganh,
                TenChuyenNganh = model.TenChuyenNganh,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = model.CreatedBy
            };

            try
            {
                _context.Nganhs.Add(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Tạo ngành thành công");
            }
            catch (Exception)
            {
                return BaseResult.Fail("Có lỗi xảy ra");
            }
        }

        public async Task<BaseResult> DeleteNganh(int id, string deletedBy)
        {
            var item = await _context.Nganhs.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (item == null)
            {
                return BaseResult.Fail("Ngành không tồn tại.");
            }

            var inLinhVuc= await _context.LinhVucNganhs.AnyAsync(x => x.NganhId == id && x.DeletedAt == null);

            if (inLinhVuc)
            {
                return BaseResult.Fail("Không thể xóa ngành vì có lĩnh vực liên quan.");
            }

            try
            {
                item.DeletedAt = DateTime.UtcNow;
                item.DeletedBy = deletedBy;
                await _context.SaveChangesAsync();
                return BaseResult.Success("Xóa ngành thành công");
            }
            catch (Exception)
            {
                return BaseResult.Fail("Có lỗi xảy ra");
            }
        }

        public async Task<BaseResult> EditNganh(int id, NganhEditVM model)
        {
            var item = await _context.Nganhs.FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);

            if (item == null)
            {
                return BaseResult.Fail("Ngành không tồn tại.");
            }

            item.MaNganh = model.MaNganh;
            item.TenChuyenNganh = model.TenChuyenNganh;
            item.TenNganh = model.TenNganh;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = model.UpdatedBy;

            try
            {
                _context.Nganhs.Update(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Chỉnh sửa ngành thành công");
            }
            catch (Exception)
            {
                return BaseResult.Fail("Có lỗi xảy ra");
            }
        }

        public async Task<PageResult<NganhVM>> GetListNganh(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var query = _context.Nganhs.AsNoTracking().Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.MaNganh.Contains(keyword) || x.TenNganh.Contains(keyword) || x.TenChuyenNganh.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var records = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new NganhVM
                {
                    Id = x.Id,
                    MaNganh = x.MaNganh,
                    TenNganh = x.TenNganh,
                    TenChuyenNganh = x.TenChuyenNganh,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return new PageResult<NganhVM>
            {
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Records = records
            };
        }

        public async Task<BaseResult<NganhVM>> GetNganhById(int id)
        {
            var result = await _context.Nganhs
                .AsNoTracking()
                .Where(x => x.Id == id && x.DeletedAt == null)
                .Select(x => new NganhVM
                {
                    Id = x.Id,
                    MaNganh = x.MaNganh,
                    TenNganh = x.TenNganh,
                    TenChuyenNganh = x.TenChuyenNganh,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return BaseResult<NganhVM>.Fail("Ngành không tồn tại.");
            }

            return BaseResult<NganhVM>.Success(result, "Lấy dữ liệu ngành thành công.");
        }
    }
}
