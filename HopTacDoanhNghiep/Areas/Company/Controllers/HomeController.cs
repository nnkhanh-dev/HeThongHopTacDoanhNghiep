using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class HomeController : Controller
    {
        [HttpGet("doanh-nghiep/dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
