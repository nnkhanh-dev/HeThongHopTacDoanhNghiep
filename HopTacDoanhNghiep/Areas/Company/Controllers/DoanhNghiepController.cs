using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class DoanhNghiepController : Controller
    {
        private readonly IDoanhNghiepCompany _doanhNghiepCompany;
        private readonly IFileStorage _fileStorage;

        public DoanhNghiepController(IDoanhNghiepCompany doanhNghiepCompany, IFileStorage fileStorage)
        {
            _doanhNghiepCompany = doanhNghiepCompany;
            _fileStorage = fileStorage;
        }

        [HttpGet("doanh-nghiep/thong-tin")]
        public async Task<IActionResult> ThongTin()
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
            {
                return Unauthorized();
            }

            var result = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy thông tin doanh nghiệp";
                return RedirectToAction(nameof(Index));
            }

            // Hiển thị trang xem thông tin (view-only)
            return View("ChiTiet", result.Data);
        }

        [HttpGet("doanh-nghiep/chinh-sua")]
        public async Task<IActionResult> ChinhSua()
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
            {
                return Unauthorized();
            }

            var result = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy thông tin doanh nghiệp";
                return RedirectToAction(nameof(ThongTin));
            }

            ViewBag.DoanhNghiep = result.Data;

            return View(new DoanhNghiepUpdateVM
            {
                TenHienThi = result.Data.TenHienThi,
                Website = result.Data.Website,
                MaSoThue = result.Data.MaSoThue,
                TenPhapLy = result.Data.TenPhapLy,
                Hotline = result.Data.Hotline,
                EmailCongTy = result.Data.EmailCongTy,
                Logo = result.Data.Logo,
                DiaChi = result.Data.DiaChi,
                GioiThieu = result.Data.GioiThieu,
                QuyMoNhanSu = result.Data.QuyMoNhanSu
            });
        }

        [HttpPost("doanh-nghiep/chinh-sua")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSua(DoanhNghiepUpdateVM model, IFormFile? LogoFile)
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                var current = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
                if (current.IsSuccess && current.Data != null)
                {
                    ViewBag.DoanhNghiep = current.Data;
                    model.Logo = string.IsNullOrWhiteSpace(model.Logo) ? current.Data.Logo : model.Logo;
                }

                return View(model);
            }

            var oldLogo = model.Logo;

            if (LogoFile != null && LogoFile.Length > 0)
            {
                var uploadResult = await _fileStorage.UploadAsync(
                    LogoFile,
                    new FileUploadOptions
                    {
                        Folder = "uploads/doanh-nghiep",
                        AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
                        MaxSizeInBytes = 5 * 1024 * 1024,
                        RenameFile = true
                    });

                if (!uploadResult.IsSuccess)
                {
                    TempData["ErrorMessage"] = "Upload ảnh logo thất bại: " + uploadResult.Message;
                    var current = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
                    if (current.IsSuccess && current.Data != null)
                    {
                        ViewBag.DoanhNghiep = current.Data;
                    }

                    return View(model);
                }

                model.Logo = uploadResult.FilePath;
            }

            model.UpdatedBy = User.Identity?.Name;

            var result = await _doanhNghiepCompany.UpdateDoanhNghiepInfo(maDoanhNghiep, model);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                var current = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
                if (current.IsSuccess && current.Data != null)
                {
                    ViewBag.DoanhNghiep = current.Data;
                }

                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.Logo) && !string.Equals(oldLogo, model.Logo, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(oldLogo))
            {
                await _fileStorage.DeleteAsync(oldLogo);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(ThongTin));
        }

        [HttpGet("doanh-nghiep/nguoi-dai-dien")]
        public async Task<IActionResult> NguoiDaiDien()
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
                return Unauthorized();

            var result = await _doanhNghiepCompany.GetNguoiDaiDienInfo(maDoanhNghiep);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy người đại diện";
                return RedirectToAction(nameof(ThongTin));
            }

            return View("NguoiDaiDienChiTiet", result.Data);
        }

        [HttpGet("doanh-nghiep/nguoi-dai-dien/chinh-sua")]
        public async Task<IActionResult> ChinhSuaNguoiDaiDien()
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
                return Unauthorized();

            var result = await _doanhNghiepCompany.GetNguoiDaiDienInfo(maDoanhNghiep);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy người đại diện";
                return RedirectToAction(nameof(ThongTin));
            }

            var vm = new NguoiDaiDienUpdateVM
            {
                HoTen = result.Data.HoTen ?? string.Empty,
                SoDienThoai = result.Data.SoDienThoai ?? string.Empty,
                Email = result.Data.Email ?? string.Empty,
                AnhNguoiDaiDien = result.Data.AnhNguoiDaiDien
            };

            ViewBag.DoanhNghiep = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
            return View("NguoiDaiDienChinhSua", vm);
        }

        [HttpPost("doanh-nghiep/nguoi-dai-dien/chinh-sua")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSuaNguoiDaiDien(NguoiDaiDienUpdateVM model, IFormFile? AnhFile)
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                var current = await _doanhNghiepCompany.GetNguoiDaiDienInfo(maDoanhNghiep);
                if (current.IsSuccess && current.Data != null)
                {
                    ViewBag.DoanhNghiep = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
                    model.AnhNguoiDaiDien = string.IsNullOrWhiteSpace(model.AnhNguoiDaiDien) ? current.Data.AnhNguoiDaiDien : model.AnhNguoiDaiDien;
                }

                return View("NguoiDaiDienChinhSua", model);
            }

            var oldAvatar = model.AnhNguoiDaiDien;

            if (AnhFile != null && AnhFile.Length > 0)
            {
                var uploadResult = await _fileStorage.UploadAsync(AnhFile, new FileUploadOptions
                {
                    Folder = "uploads/nguoi-dai-dien",
                    AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
                    MaxSizeInBytes = 5 * 1024 * 1024,
                    RenameFile = true
                });

                if (!uploadResult.IsSuccess)
                {
                    TempData["ErrorMessage"] = "Upload ảnh thất bại: " + uploadResult.Message;
                    ViewBag.DoanhNghiep = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
                    return View("NguoiDaiDienChinhSua", model);
                }

                model.AnhNguoiDaiDien = uploadResult.FilePath;
            }

            model.UpdatedBy = User.Identity?.Name;

            var result = await _doanhNghiepCompany.UpdateNguoiDaiDienInfo(maDoanhNghiep, model);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                ViewBag.DoanhNghiep = await _doanhNghiepCompany.GetDoanhNghiepInfo(maDoanhNghiep);
                return View("NguoiDaiDienChinhSua", model);
            }

            if (!string.IsNullOrWhiteSpace(model.AnhNguoiDaiDien) && !string.Equals(oldAvatar, model.AnhNguoiDaiDien, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(oldAvatar))
            {
                await _fileStorage.DeleteAsync(oldAvatar);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(NguoiDaiDien));
        }
    }
}
