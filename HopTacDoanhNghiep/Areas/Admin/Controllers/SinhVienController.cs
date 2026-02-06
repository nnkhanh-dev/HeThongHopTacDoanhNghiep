using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Enums.NhapDuLieu;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SinhVienController : Controller
    {
        private readonly ISinhVienAdmin _sinhVien;
        private readonly INhapDuLieuAdmin _nhapDuLieu;

        public SinhVienController(ISinhVienAdmin sinhVien, INhapDuLieuAdmin nhapDuLieu)
        {
            _sinhVien = sinhVien;
            _nhapDuLieu = nhapDuLieu;
        }

        [HttpGet("admin/sinh-vien")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null, string? khoa = null, string? chuyenNganh = null)
        {
            if(pageIndex < 1) pageIndex = 1;
            if(pageSize < 1) pageSize = 10;
            var sinhViens = await _sinhVien.GetListSinhVien(pageIndex, pageSize, keyword, khoa, chuyenNganh);
            return View(sinhViens);
        }

        [HttpGet("admin/sinh-vien/chi-tiet/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _sinhVien.GetSinhVienById(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpGet("admin/sinh-vien/nhap-du-lieu")]
        public async Task<IActionResult> ImportData(int pageIndex = 1, int pageSize = 10, string? keyword = null, NhapDuLieuStatus? status = null)
        {
            if(pageIndex < 1) pageIndex = 1;
            if(pageSize < 1) pageSize = 10;
            var result = await _nhapDuLieu.GetListLichSuNhapDuLieu(NhapDuLieuType.SinhVien, pageIndex, pageSize, keyword, status);
            return View(result);
        }

        [HttpPost("admin/sinh-vien/nhap-du-lieu/upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            var result = await _nhapDuLieu.UploadSinhVienExcel(file);
            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }
            return Json(new { success = true, message = "Tải lên tệp Excel thành công và đang xử lý dữ liệu." });
        }
    }
}