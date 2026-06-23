using HopTacDoanhNghiep.Areas.Officer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Officer.Controllers
{
    [Area("Officer")]
    [Authorize(Roles = "Officer")]
    public class ThongTinController : Controller
    {
        private readonly IBaiVietOfficer _baiViet;

        public ThongTinController(IBaiVietOfficer baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/can-bo/thong-tin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/can-bo/thong-tin/tin-tuc")]
        public async Task<IActionResult> TinTuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "tin-tuc");
            return View(result);
        }

        [HttpGet("/can-bo/thong-tin/thong-bao")]
        public async Task<IActionResult> ThongBao(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "thong-bao");
            return View(result);
        }

        [HttpGet("/can-bo/thong-tin/ute-can-bo")]
        public async Task<IActionResult> UTEDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "ute-can-bo");
            return View(result);
        }

        [HttpGet("/can-bo/thong-tin/hop-tac-can-bo")]
        public async Task<IActionResult> HopTacDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "hop-tac-can-bo");
            return View(result);
        }

        [HttpGet("/can-bo/thong-tin/tin-tuc/{slug}")]
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

        [HttpGet("/can-bo/thong-tin/thong-bao/{slug}")]
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

        [HttpGet("/can-bo/thong-tin/ute-can-bo/{slug}")]
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

        [HttpGet("/can-bo/thong-tin/hop-tac-can-bo/{slug}")]
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
