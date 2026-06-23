using HopTacDoanhNghiep.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IDashboardAdmin _dashboardAdmin;

        public HomeController(IDashboardAdmin dashboardAdmin)
        {
            _dashboardAdmin = dashboardAdmin;
        }

        [HttpGet("admin/dashboard")]
        public async Task<IActionResult> Index(int? year)
        {
            var dashboardData = await _dashboardAdmin.GetDashboardData(year);
            return View(dashboardData);
        }
    }
}
