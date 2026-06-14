using HopTacDoanhNghiep.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TinTuyenDungController : Controller
    {
        private readonly ITinTuyenDungAdmin _tinTuyenDungAdmin;

        public TinTuyenDungController(ITinTuyenDungAdmin tinTuyenDungAdmin)
        {
            _tinTuyenDungAdmin = tinTuyenDungAdmin;
        }

        [HttpGet("/admin/tin-tuyen-dung")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _tinTuyenDungAdmin.GetListTinTuyenDung(pageIndex, pageSize, keyword);
            return View(result);
        }

        [HttpGet("/admin/tin-tuyen-dung/{MaTTD}")]
        public async Task<IActionResult> Details(int MaTTD)
        {
            var result = await _tinTuyenDungAdmin.GetTinTuyenDungByMaTTD(MaTTD);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return View(result);
        }
    }
}
