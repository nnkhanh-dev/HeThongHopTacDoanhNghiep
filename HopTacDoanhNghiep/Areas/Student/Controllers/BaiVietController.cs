using HopTacDoanhNghiep.Areas.Student.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class BaiVietController : Controller
    {
        private readonly IBaiVietStudent _baiViet;

        public BaiVietController(IBaiVietStudent baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/sinh-vien/bai-viet")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null, string? danhMucSlug = null)
        {
            var baiViets = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, danhMucSlug);
            return View(baiViets);
        }

        [HttpGet("/sinh-vien/bai-viet/{slug}")]
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
