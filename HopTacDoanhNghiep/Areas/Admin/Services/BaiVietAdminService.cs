using HopTacDoanhNghiep.Areas.Admin.ViewModels.BaiViet;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums;
using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.Services;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Areas.Admin.Services
{
    public class BaiVietAdminService : IBaiVietAdmin
    {
        private readonly AppDbContext _context;
        private readonly ISlug _slugService;
        private readonly IFileStorage _fileStorage;

        public BaiVietAdminService(AppDbContext context, ISlug slugService, IFileStorage fileStorage)
        {
            _context = context;
            _slugService = slugService;
            _fileStorage = fileStorage;
        }

        public async Task<BaseResult> CreateBaiViet(BaiVietCreateVM baiViet)
        {
            if (baiViet == null)
                return BaseResult.Fail("Dữ liệu bài viết không hợp lệ");

            try
            {
                // Generate unique slug
                var slug = await _slugService.GenerateUniqueSlugAsync(
                    baiViet.TieuDe,
                    _context.BaiViets.AsNoTracking(),
                    x => x.Slug
                );

                string? imagePath = null;
                if (baiViet.AnhMinhHoa != null)
                {
                    var upload = await _fileStorage.UploadAsync(baiViet.AnhMinhHoa, new FileUploadOptions { Folder = "uploads/bai-viet" });
                    if (!upload.IsSuccess)
                        return BaseResult.Fail("Upload ảnh thất bại: " + upload.Message);

                    imagePath = upload.FilePath;
                }

                var entity = new Models.BaiViet
                {
                    TieuDe = baiViet.TieuDe,
                    AnhMinhHoa = imagePath,
                    TacGia = baiViet.TacGia ?? "Admin",
                    NoiDung = baiViet.NoiDung,
                    Slug = slug,
                    TuKhoa = baiViet.TuKhoa ?? "",
                    TrangThai = baiViet.TrangThai,
                    MaDanhMuc = baiViet.MaDanhMuc ?? 0,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = baiViet.CreatedBy
                };

                _context.BaiViets.Add(entity);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Tạo bài viết thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Tạo bài viết thất bại. Lỗi: " + ex.Message);
            }
        }

        public async Task<BaseResult> DeleteBaiViet(int maBaiViet, string deletedBy)
        {
            var item = await _context.BaiViets.FirstOrDefaultAsync(x => x.MaBaiViet == maBaiViet && x.DeletedAt == null);

            if(item == null)
            {
                return BaseResult.Fail("Bài viết không tồn tại");
            }

            try
            {
                item.DeletedAt = DateTime.UtcNow;
                item.DeletedBy = deletedBy;
                await _context.SaveChangesAsync();

                return BaseResult.Success("Xóa bài viết thành công");
            }
            catch(Exception ex)
            {
                return BaseResult.Fail("Xóa bài viết thất bại. Lỗi: " + ex.Message);
            }
        }

        public async Task<BaseResult> EditBaiViet(int maBaiViet, BaiVietEditVM baiViet)
        {
            if (baiViet == null)
                return BaseResult.Fail("Dữ liệu bài viết không hợp lệ");

            var item = await _context.BaiViets.FirstOrDefaultAsync(x => x.MaBaiViet == maBaiViet && x.DeletedAt == null);
            if (item == null)
                return BaseResult.Fail("Bài viết không tồn tại");

            try
            {
                // Kiểm tra và cập nhật slug nếu tiêu đề thay đổi
                if (!string.Equals(item.TieuDe, baiViet.TieuDe, StringComparison.Ordinal))
                {
                    var slug = await _slugService.GenerateUniqueSlugAsync(
                        baiViet.TieuDe,
                        _context.BaiViets.AsNoTracking().Where(x => x.MaBaiViet != maBaiViet),
                        x => x.Slug
                    );
                    
                    item.TieuDe = baiViet.TieuDe;
                    item.Slug = slug;
                }

                item.NoiDung = baiViet.NoiDung;
                item.MaDanhMuc = baiViet.MaDanhMuc;
                item.TrangThai = baiViet.TrangThai;
                item.UpdatedAt = DateTime.UtcNow;
                item.UpdatedBy = baiViet.UpdatedBy;
                item.TuKhoa = baiViet.TuKhoa ?? "";

                if (baiViet.AnhMinhHoa != null)
                {
                    var upload = await _fileStorage.UploadAsync(baiViet.AnhMinhHoa, new FileUploadOptions { Folder = "uploads/bai-viet" });
                    if (!upload.IsSuccess)
                        return BaseResult.Fail("Upload ảnh thất bại: " + upload.Message);
                    
                    item.AnhMinhHoa = upload.FilePath;
                }

                _context.BaiViets.Update(item);
                await _context.SaveChangesAsync();

                return BaseResult.Success("Cập nhật bài viết thành công");
            }
            catch (Exception ex)
            {
                return BaseResult.Fail("Cập nhật bài viết thất bại. Lỗi: " + ex.Message);
            }
        }

        public async Task<BaseResult<BaiVietVM>> GetBaiVietById(int maBaiViet)
        {
            var item = await _context.BaiViets
                .AsNoTracking()
                .Where(x => x.MaBaiViet == maBaiViet && x.DeletedAt == null)
                .Select(x => new BaiVietVM
                {
                    MaBaiViet = x.MaBaiViet,
                    TieuDe = x.TieuDe,
                    AnhMinhHoa = x.AnhMinhHoa,
                    TacGia = x.TacGia,
                    NoiDung = x.NoiDung,
                    Slug = x.Slug,
                    TuKhoa = x.TuKhoa,
                    TrangThai = x.TrangThai,
                    MaDanhMuc = x.MaDanhMuc,
                    DanhMuc = x.DanhMuc.Ten,
                    CreatedAt = x.CreatedAt,
                    DeletedAt = x.DeletedAt
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return BaseResult<BaiVietVM>.Fail("Bài viết không tồn tại");
            }

            return BaseResult<BaiVietVM>.Success(item, "Lấy dữ liệu bài viết thành công");
        }

        public async Task<PageResult<BaiVietVM>> GetListBaiViet(int pageIndex, int pageSize, string? keyword, int? MaDanhMuc = null, BaiVietStatus? status = null)
        {
            var query = _context.BaiViets.Where(x => x.DeletedAt == null).AsNoTracking();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TieuDe.Contains(keyword) || x.TacGia.Contains(keyword));
            }
            
            if (MaDanhMuc.HasValue)
            {
                query = query.Where(x => x.MaDanhMuc == MaDanhMuc.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(x => x.TrangThai == status.Value);
            }

            var total = await query.CountAsync();

            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BaiVietVM
                {
                    MaBaiViet = x.MaBaiViet,
                    TieuDe = x.TieuDe,
                    AnhMinhHoa = x.AnhMinhHoa,
                    TacGia = x.TacGia,
                    NoiDung = x.NoiDung,
                    Slug = x.Slug,
                    TrangThai = x.TrangThai,
                    MaDanhMuc = x.MaDanhMuc,
                    DanhMuc = x.DanhMuc.Ten,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy
                })
                .ToListAsync();

            return new PageResult<BaiVietVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = total,
                Records = items
            };
        }

        
    }
}
