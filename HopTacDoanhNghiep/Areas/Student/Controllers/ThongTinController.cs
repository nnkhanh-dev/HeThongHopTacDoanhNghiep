using HopTacDoanhNghiep.Areas.Student.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class ThongTinController : Controller
    {
        private readonly IBaiVietStudent _baiViet;

        public ThongTinController(IBaiVietStudent baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/sinh-vien/sinh-vien/thong-tin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/sinh-vien/thong-tin/tin-tuc")]
        public async Task<IActionResult> TinTuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "tin-tuc");
            return View(result);
        }

        [HttpGet("/sinh-vien/thong-tin/thong-bao")]
        public async Task<IActionResult> ThongBao(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "thong-bao");
            return View(result);
        }

        [HttpGet("/sinh-vien/thong-tin/ute-doanh-nghiep")]
        public async Task<IActionResult> UTEDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "ute-doanh-nghiep");
            return View(result);
        }

        [HttpGet("/sinh-vien/thong-tin/hop-tac-doanh-nghiep")]
        public async Task<IActionResult> HopTacDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "hop-tac-doanh-nghiep");
            return View(result);
        }

        [HttpGet("/sinh-vien/thong-tin/tin-tuc/{slug}")]
        public async Task<IActionResult> BaiVietTinTuc(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }

        [HttpGet("/sinh-vien/thong-tin/thong-bao/{slug}")]
        public async Task<IActionResult> BaiVietThongBao(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }

        [HttpGet("/sinh-vien/thong-tin/ute-doanh-nghiep/{slug}")]
        public async Task<IActionResult> BaiVietUTEDoanhNghiep(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }

        [HttpGet("/sinh-vien/thong-tin/hop-tac-doanh-nghiep/{slug}")]
        public async Task<IActionResult> BaiVietHopTacDoanhNghiep(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }
    }
}
