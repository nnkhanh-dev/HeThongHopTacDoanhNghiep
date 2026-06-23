using HopTacDoanhNghiep.Areas.Student.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class DoanhNghiepController : Controller
    {
        private readonly IDoanhNghiepStudent _doanhNghiepStudent;
        private readonly ITinTuyenDungStudent _tinTuyenDungStudent;

        public DoanhNghiepController(IDoanhNghiepStudent doanhNghiepStudent, ITinTuyenDungStudent tinTuyenDungStudent)
        {
            _doanhNghiepStudent = doanhNghiepStudent;
            _tinTuyenDungStudent = tinTuyenDungStudent;
        }

        [HttpGet("/sinh-vien/doanh-nghiep")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 9, string? keyword = null)
        {
            var result = await _doanhNghiepStudent.GetListDoanhNghiep(pageIndex, pageSize, keyword);
            ViewData["Title"] = "Doanh nghiệp";
            ViewData["OgDescription"] = "Danh sách doanh nghiệp dành cho sinh viên.";
            return View(result);
        }

        [HttpGet("/sinh-vien/doanh-nghiep/{maDN}")]
        public async Task<IActionResult> Details(string maDN, int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _doanhNghiepStudent.GetDoanhNghiepByMaDN(maDN);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy doanh nghiệp.";
                return RedirectToAction(nameof(Index));
            }

            var tinTuyenDungResult = await _tinTuyenDungStudent.GetTinTuyenDungByCompanyId(maDN, pageIndex, pageSize, keyword);

            result.Data.TinTuyenDung = tinTuyenDungResult;

            ViewData["Title"] = result.Data.TenHienThi;
            ViewData["OgDescription"] = result.Data.GioiThieu ?? result.Data.TenHienThi;
            ViewData["OgImage"] = result.Data.Logo;
            return View(result.Data);
        }

    }
}
