using HopTacDoanhNghiep.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonViController : Controller
    {
        private readonly IDonViAdmin _donViAdmin;

        public DonViController(IDonViAdmin donViAdmin)
        {
            _donViAdmin = donViAdmin;
        }

        [HttpGet("/admin/don-vi")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/admin/don-vi/danh-sach")]
        public async Task<IActionResult> GetListDonVi(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _donViAdmin.GetDonViAsync(pageIndex, pageSize, keyword);
            return Json(result);
        }
    }
}
