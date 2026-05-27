using HopTacDoanhNghiep.Areas.Student.Services;
using HopTacDoanhNghiep.Areas.Student.ViewModels.DonUngTuyen;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class DonUngTuyenController : Controller
    {
        private readonly IDonUngTuyenStudent _donUngTuyenStudent;
        private readonly IFileStorage _fileStorage;

        public DonUngTuyenController(IDonUngTuyenStudent donUngTuyenStudent, IFileStorage fileStorage)
        {
            _donUngTuyenStudent = donUngTuyenStudent;
            _fileStorage = fileStorage;
        }

        [HttpGet("/sinh-vien/don-ung-tuyen")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var maSinhVien = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return NotFound("Không tìm thấy thông tin sinh viên");
            }

            var result = await _donUngTuyenStudent.GetListDonUngTuyen(pageIndex, pageSize, keyword, maSinhVien);
            return View(result);
        }

        [HttpGet("/sinh-vien/don-ung-tuyen/nop-ho-so/{id:int}")]
        public IActionResult Apply(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Tin tuyển dụng không hợp lệ");
            }

            return View(new DonUngTuyenCreateVM { MaTTD = id });
        }

        [HttpPost("/sinh-vien/don-ung-tuyen")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply([FromForm] DonUngTuyenCreateVM model, [FromForm] IFormFile hoSoUngTuyen)
        {
            var maSinhVien = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin sinh viên" });
            }

            model.MaSV = maSinhVien;

            if (!ModelState.IsValid)
            {
                return Json (new{ success = false, message = "Dữ liệu không hợp lệ" });
            }

            if (hoSoUngTuyen == null || hoSoUngTuyen.Length == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn hồ sơ ứng tuyển" });
            }

            var uploadResult = await _fileStorage.UploadAsync(
                hoSoUngTuyen,
                new FileUploadOptions
                {
                    Folder = "uploads/don-ung-tuyen",
                    AllowedExtensions = new[] { ".pdf", ".doc", ".docx" },
                    MaxSizeInBytes = 10 * 1024 * 1024,
                    RenameFile = true
                });

            if (!uploadResult.IsSuccess || string.IsNullOrWhiteSpace(uploadResult.FilePath))
            {
                ModelState.AddModelError(string.Empty, uploadResult.Message ?? "Upload hồ sơ thất bại");
                return View(model);
            }

            model.HoSoUngTuyen = uploadResult.FilePath;

            var result = await _donUngTuyenStudent.ApplyDonUngTuyen(model);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = result.Message });
        }

        [HttpGet("/sinh-vien/don-ung-tuyen/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Tin tuyển dụng không hợp lệ");
            }

            var maSinhVien = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return NotFound("Không tìm thấy thông tin sinh viên");
            }

            var result = await _donUngTuyenStudent.GetDonUngTuyenById(id, maSinhVien);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound(result.Message);
            }

            return View(result.Data);
        }
    }
}
