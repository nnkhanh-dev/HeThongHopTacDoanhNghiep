using HopTacDoanhNghiep.Areas.Officer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Officer.Controllers
{
    [Area("Officer")]
    [Authorize(Roles = "Officer")]
    public class BaiVietController : Controller
    {
        private readonly IBaiVietOfficer _baiViet;

        public BaiVietController(IBaiVietOfficer baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/can-bo/bai-viet")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null, string? danhMucSlug = null)
        {
            var baiViets = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, danhMucSlug);
            return View(baiViets);
        }

        [HttpGet("/can-bo/bai-viet/{slug}")]
        public async Task<IActionResult> Details(string slug)
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
