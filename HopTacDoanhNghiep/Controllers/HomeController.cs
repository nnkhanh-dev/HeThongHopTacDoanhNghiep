using System.Diagnostics;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaiViet _baiViet;

        public HomeController(IBaiViet baiViet)
        {
            _baiViet = baiViet;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/lien-he")]
        public IActionResult LienHe()
        {
            return View();
        }
    }
}
