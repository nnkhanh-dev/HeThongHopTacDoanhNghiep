using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Student.Services;
using HopTacDoanhNghiep.Areas.Student.ViewModels.SinhVien;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.Services;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class SinhVienController : Controller
    {
        private readonly ISinhVienStudent _sinhVienStudent;
        private readonly IFileStorage _fileStorage;

        public SinhVienController(ISinhVienStudent sinhVienStudent, IFileStorage fileStorage)
        {
            _sinhVienStudent = sinhVienStudent;
            _fileStorage = fileStorage;
        }


        [HttpGet("/sinh-vien/tai-khoan")]
        public async Task<IActionResult> Index()
        {
            var maSinhVien = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return Unauthorized();
            }

            var result = await _sinhVienStudent.GetStudentInfo(maSinhVien);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy thông tin sinh viên";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpGet("/sinh-vien/chinh-sua")]
        public async Task<IActionResult> Edit()
        {
            var maSinhVien = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return Unauthorized();
            }
            var result = await _sinhVienStudent.GetStudentInfo(maSinhVien);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy thông tin sinh viên.";
                return RedirectToAction("Index");
            }

            var data = new EditSinhVienVM
            {
                HoSoNangLuc = result.Data.HoSoNangLuc,
                AnhThe = result.Data.AnhThe,
                Email = result.Data.Email,
                SDT = result.Data.SDT
            };
            return View(data);
        }

        [HttpPost("/sinh-vien/chinh-sua")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSinhVienVM editSinhVienVM, IFormFile? AvatarFile)
        {
            var maSinhVien = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maSinhVien))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return View(editSinhVienVM);
            }
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var uploadResult = await _fileStorage.UploadAsync(
                    AvatarFile,
                    new FileUploadOptions
                    {
                        Folder = "uploads/avatar",
                        AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
                        MaxSizeInBytes = 5 * 1024 * 1024,
                        RenameFile = true
                    });

                if (!uploadResult.IsSuccess)
                {
                    TempData["ErrorMessage"] = "Upload avatar thất bại: " + uploadResult.Message;
                    return View(editSinhVienVM);
                }

                editSinhVienVM.AnhThe = uploadResult.FilePath; // reuse AnhThe field for avatar path
            }
            var result = await _sinhVienStudent.EditStudentInfo(maSinhVien, editSinhVienVM);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message ?? "Cập nhật thông tin sinh viên thất bại.";
                return View(editSinhVienVM);
            }
            TempData["SuccessMessage"] = "Cập nhật thông tin sinh viên thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}

