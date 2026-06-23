using HopTacDoanhNghiep.Areas.Officer.ViewModels.HomeVM;
using HopTacDoanhNghiep.Areas.Officer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Officer.Controllers
{
    [Area("Officer")]
    [Authorize(Roles = "Officer")]
    public class HomeController : Controller
    {
        private readonly IBaiVietOfficer _baiViet;

        public HomeController(IBaiVietOfficer baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("can-bo/trang-chu")]
        public async Task<IActionResult> Index()
        {
            var tinResult = await _baiViet.GetListBaiViet(1, 12, null, "tin-tuc");
            var thongBaoResult = await _baiViet.GetListBaiViet(1, 12, null, "thong-bao");
            var baiVietResult = await _baiViet.GetListBaiViet(1, 12, null, "bai-viet");

            var model = new HomeVM
            {
                TinTucs = tinResult?.Records?.ToList(),
                ThongBaos = thongBaoResult?.Records?.ToList(),
                BaiViets = baiVietResult?.Records?.ToList()
            };

            return View(model);
        }
    }
}
