using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.Areas.Company.ViewModels.BaiViet;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

using HopTacDoanhNghiep.Areas.Company.Services;

namespace HopTacDoanhNghiep.Areas.Company.Services
{
    public class BaiVietCompanyService : IBaiVietCompany
    {
        private readonly AppDbContext _context;

        public BaiVietCompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<BaiVietVM>> GetBaiVietBySlug(string slug)
        {
            var item = await _context.BaiViets
                .AsNoTracking()
                .Where(x => x.Slug == slug && x.TrangThai == BaiVietStatus.XuatBan && x.DeletedAt == null)
                .Select(x => new BaiVietVM
                {
                    TieuDe = x.TieuDe,
                    AnhMinhHoa = x.AnhMinhHoa,
                    TacGia = x.TacGia,
                    NoiDung = x.NoiDung,
                    Slug = x.Slug,
                    DanhMuc = x.DanhMuc.Ten,
                    DanhMucSlug = x.DanhMuc.Slug,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();


            if (item == null)
            {
                return BaseResult<BaiVietVM>.Fail("Không tìm thấy bài viết");
            }

            return BaseResult<BaiVietVM>.Success(item, "Lấy dữ liệu bài viết thành công");
        }

        public async Task<BaseResult<DanhMucBaiVietVM>> GetDanhMucBySlug(string slug)
        {
            var item = await _context.DanhMucBaiViets
                .AsNoTracking()
                .Where(x => x.Slug == slug && x.DeletedAt == null)
                .Select(x => new DanhMucBaiVietVM
                {
                    Ten = x.Ten,
                    Slug = x.Slug,
                    MoTa = x.MoTa
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return BaseResult<DanhMucBaiVietVM>.Fail("Không tìm thấy danh mục bài viết");
            }

            return BaseResult<DanhMucBaiVietVM>.Success(item, "Lấy dữ liệu danh mục bài viết thành công");
        }

        public async Task<PageResult<BaiVietVM>> GetListBaiViet(int pageIndex, int pageSize, string? keyword, string? danhMucSlug)
        {
            var query = _context.BaiViets.AsNoTracking().Where(x => x.TrangThai == BaiVietStatus.XuatBan && x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TieuDe.Contains(keyword) || x.NoiDung.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(danhMucSlug))
            {
                if (danhMucSlug == "bai-viet")
                {
                    query = query.Where(x => x.DanhMuc.Slug != "tin-tuc"
                                          && x.DanhMuc.Slug != "thong-bao");
                }
                else
                {
                    query = query.Where(x => x.DanhMuc.Slug == danhMucSlug);
                }
            }

            var totalRecords = await query.CountAsync();

            var list = await query.OrderByDescending(x => x.CreatedAt)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => new BaiVietVM
                                    {
                                        TieuDe = x.TieuDe,
                                        AnhMinhHoa = x.AnhMinhHoa,
                                        TacGia = x.TacGia,
                                        NoiDung = StripHtml(x.NoiDung, 200),
                                        Slug = x.Slug,
                                        DanhMuc = x.DanhMuc.Ten,
                                        DanhMucSlug = x.DanhMuc.Slug,
                                        CreatedAt = x.CreatedAt
                                    })
                                    .ToListAsync();

            return new PageResult<BaiVietVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = list
            };
        }


        private static string StripHtml(string html, int maxLength = 200)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;

            // Decode HTML entities (&agrave; -> à)
            var decoded = WebUtility.HtmlDecode(html);

            // Remove HTML tags
            var text = Regex.Replace(decoded, "<.*?>", string.Empty);

            // Normalize spaces
            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text.Length <= maxLength
                ? text
                : text.Substring(0, maxLength) + "…";
        }

        public async Task<PageResult<BaiVietVM>> GetListRelatedBaiViet(int pageIndex, int pageSize, string baiVietSlug ,string? keyword, string? danhMucSlug)
        {
            var query = _context.BaiViets.AsNoTracking().Where(x => x.TrangThai == BaiVietStatus.XuatBan && x.DeletedAt == null && x.Slug != baiVietSlug);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.TieuDe.Contains(keyword) || x.NoiDung.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(danhMucSlug))
            {
                query = query.Where(x => x.DanhMuc.Slug == danhMucSlug);
            }

            var totalRecords = await query.CountAsync();

            var list = await query.OrderByDescending(x => x.CreatedAt)
                                    .Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(x => new BaiVietVM
                                    {
                                        TieuDe = x.TieuDe,
                                        AnhMinhHoa = x.AnhMinhHoa,
                                        TacGia = x.TacGia,
                                        NoiDung = StripHtml(x.NoiDung, 200),
                                        Slug = x.Slug,
                                        DanhMuc = x.DanhMuc.Ten,
                                        DanhMucSlug = x.DanhMuc.Slug,
                                        CreatedAt = x.CreatedAt
                                    })
                                    .ToListAsync();

            return new PageResult<BaiVietVM>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Records = list
            };
        }
    }  

}
