using HopTacDoanhNghiep.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ChucVuController : Controller
    {
        private readonly IChucVuAdmin _chucVuAdmin;

        public ChucVuController(IChucVuAdmin chucVuAdmin)
        {
            _chucVuAdmin = chucVuAdmin;
        }

        [HttpGet("/admin/chuc-vu")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/admin/chuc-vu/danh-sach")]
        public async Task<IActionResult> GetListChucVu(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _chucVuAdmin.GetChucVuAsync(pageIndex, pageSize, keyword);
            return Json(result);
        }
    }
}
