using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Enums.NhapDuLieu;
using HopTacDoanhNghiep.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoanhNghiepController : Controller
    {
        private readonly IDoanhNghiepAdmin _doanhNghiep;
        private readonly INhapDuLieuAdmin _nhapDuLieu;

        public DoanhNghiepController(IDoanhNghiepAdmin doanhNghiep, INhapDuLieuAdmin nhapDuLieu)
        {
            _doanhNghiep = doanhNghiep;
            _nhapDuLieu = nhapDuLieu;
        }

        [HttpGet("admin/doanh-nghiep")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            var doanhNghieps = await _doanhNghiep.GetListDoanhNghiep(pageIndex, pageSize, keyword);
            return View(doanhNghieps);
        }


        [HttpGet("admin/doanh-nghiep/chi-tiet/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _doanhNghiep.GetDoanhNghiepById(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpGet("admin/doanh-nghiep/nhap-du-lieu")]
        public async Task<IActionResult> ImportData(int pageIndex = 1, int pageSize = 10, string? keyword = null, NhapDuLieuStatus? status = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            var result = await _nhapDuLieu.GetListLichSuNhapDuLieu(NhapDuLieuType.DoanhNghiep, pageIndex, pageSize, keyword, status);
            return View(result);
        }

        [HttpPost("admin/doanh-nghiep/nhap-du-lieu/upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _nhapDuLieu.UploadDoanhNghiepExcel(file, userId);
            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = "Tải lên tệp Excel thành công và đang xử lý dữ liệu." });
        }
    }
}
