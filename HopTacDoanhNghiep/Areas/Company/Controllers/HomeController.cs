using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep;
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
        private readonly IDoanhNghiepCompany _doanhNghiepCompany;
        private readonly IFileStorage _fileStorage;

        public HomeController(IDoanhNghiepCompany doanhNghiepCompany, IFileStorage fileStorage)
        {
            _doanhNghiepCompany = doanhNghiepCompany;
            _fileStorage = fileStorage;
        }

        [HttpGet("doanh-nghiep/dashboard")]
        [HttpGet("doanh-nghiep")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
