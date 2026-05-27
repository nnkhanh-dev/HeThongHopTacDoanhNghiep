using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using HopTacDoanhNghiep.Areas.Officer.Services;
using HopTacDoanhNghiep.Areas.Officer.ViewModels;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HopTacDoanhNghiep.Areas.Officer.Controllers
{
    [Area("Officer")]
    [Authorize(Roles = "Officer")]
    public class DoanhNghiepController : Controller
    {
        private readonly IDoanhNghiepOfficer _service;

        public DoanhNghiepController(IDoanhNghiepOfficer service)
        {
            _service = service;
        }

        [HttpGet("can-bo/doanh-nghiep/danh-sach")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            var model = await _service.GetListDoanhNghiep(pageIndex, pageSize, keyword);

            return View(model);
        }

        [HttpGet("can-bo/doanh-nghiep/dang-ky")]
        public async Task<IActionResult> DangKyDoanhNghiep(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            var maCB = User.Identity?.Name;

            var model = await _service.GetListDangKyDoanhNghiep(pageIndex, pageSize, keyword, maCB);

            return View(model);
        }

        [HttpGet("can-bo/dang-ky-doanh-nghiep/{MaDN}")]
        public async Task<IActionResult> Details(string MaDN)
        {
            var result = await _service.GetDoanhNghiepByMaDN(MaDN);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return View(result.Data);
        }

        [HttpGet("can-bo/doanh-nghiep/dang-ky/{MaDN}")]
        public async Task<IActionResult> ChiTietDangKyDoanhNghiep(string MaDN)
        {
            var result = await _service.GetDoanhNghiepByMaDN(MaDN);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return View(result.Data);
        }
    }
}
