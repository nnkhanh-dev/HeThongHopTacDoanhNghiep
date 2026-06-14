using HopTacDoanhNghiep.Areas.Student.Services;
using HopTacDoanhNghiep.Areas.Student.ViewModels.HomeVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly IBaiVietStudent _baiViet;
        private readonly ITinTuyenDungStudent _viecLam;

        public HomeController(IBaiVietStudent baiViet, ITinTuyenDungStudent viecLam)
        {
            _baiViet = baiViet;
            _viecLam = viecLam;
        }

        [HttpGet("sinh-vien/trang-chu")]
        [HttpGet("sinh-vien")]
        public async Task<IActionResult> Index()
        {
            var tinResult = await _baiViet.GetListBaiViet(1, 12, null, "tin-tuc");
            var thongBaoResult = await _baiViet.GetListBaiViet(1, 12, null, "thong-bao");
            var viecResult = await _viecLam.GetListTinTuyenDung(1, 6, null, null, null, null, null, null, null, null);
            var baiVietResult = await _baiViet.GetListBaiViet(1, 12, null, "bai-viet");

            var model = new HomeVM
            {
                TinTucs = tinResult?.Records?.ToList(),
                ThongBaos = thongBaoResult?.Records?.ToList(),
                ViecLams = viecResult?.Records?.ToList(),
                BaiViets = baiVietResult?.Records?.ToList()
            };

            return View(model);
        }
    }
}
