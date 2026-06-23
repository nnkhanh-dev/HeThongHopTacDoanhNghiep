using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.Areas.Company.ViewModels.HomeVM;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HopTacDoanhNghiep.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class HomeController : Controller
    {
        private readonly IBaiVietCompany _baiViet;
        private readonly ITinTuyenDungCompany _viecLam;

        public HomeController(IBaiVietCompany baiViet, ITinTuyenDungCompany viecLam)
        {
            _baiViet = baiViet;
            _viecLam = viecLam;
        }

        [HttpGet("doanh-nghiep/trang-chu")]
        [HttpGet("doanh-nghiep")]
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
