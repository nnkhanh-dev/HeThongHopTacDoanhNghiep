using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Officer.Controllers
{
    [Area("Officer")]
    [Authorize(Roles = "Officer")]
    public class HomeController : Controller
    {
        [HttpGet("can-bo/dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
