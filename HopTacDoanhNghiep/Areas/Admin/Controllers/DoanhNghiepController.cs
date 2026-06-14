using HopTacDoanhNghiep.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DoanhNghiepController : Controller
    {
        private readonly IDoanhNghiepAdmin _doanhNghiep;

        public DoanhNghiepController(IDoanhNghiepAdmin doanhNghiep)
        {
            _doanhNghiep = doanhNghiep;
        }

        [HttpGet("/admin/doanh-nghiep")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 9, string? keyword = null)
        {
            var result = await _doanhNghiep.GetListDoanhNghiep(pageIndex, pageSize, keyword);
            ViewData["Title"] = "Doanh nghiệp";
            ViewData["OgDescription"] = "Danh sách doanh nghiệp dành cho sinh viên.";
            return View(result);
        }

        [HttpGet("/admin/doanh-nghiep/{maDN}")]
        public async Task<IActionResult> Details(string maDN)
        {
            var result = await _doanhNghiep.GetDoanhNghiepByMaDN(maDN);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy doanh nghiệp.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = result.Data.TenHienThi;
            ViewData["OgDescription"] = result.Data.GioiThieu ?? result.Data.TenHienThi;
            ViewData["OgImage"] = result.Data.Logo;
            return View(result.Data);
        }
    }
}
