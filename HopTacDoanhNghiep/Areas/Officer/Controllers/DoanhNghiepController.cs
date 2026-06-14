using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Officer.Services;
using HopTacDoanhNghiep.Areas.Officer.ViewModels;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.Services;
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
        private readonly IFileStorage _fileStorage;

        public DoanhNghiepController(IDoanhNghiepOfficer service, IFileStorage fileStorage)
        {
            _service = service;
            _fileStorage = fileStorage;
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

        [HttpGet("can-bo/doanh-nghiep/{MaDN}")]
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

        [HttpGet("can-bo/doanh-nghiep/hop-tac")]
        public async Task<IActionResult> HopTacDonVi(int pageIndex = 1, int pageSize = 10, string keyword = "")
        {
            var maCB = User.Identity?.Name;
            var model = await _service.GetListHopTacDonVi(pageIndex, pageSize, keyword, maCB);

            return View(model);
        }

        [HttpGet("can-bo/doanh-nghiep/hop-tac/{MaHTDV}")]
        public async Task<IActionResult> ChiTietHopTacDonVi(int MaHTDV)
        {
            var result = await _service.GetHopTacDonViByMaHTDV(MaHTDV);
            if (!result.IsSuccess)
                return NotFound(result.Message);

            return View(result.Data);
        }

        [HttpPost("can-bo/doanh-nghiep/cap-nhat-trang-thai-hop-tac-don-vi/{MaHTDV}")]
        public async Task<IActionResult> UpdateTrangThaiHopTacDV(int MaHTDV, HopTacDonViStatus trangThai)
        {
            var maCB = User.Identity?.Name;
            var result = await _service.UpdateTrangThaiHopTacDV(MaHTDV, trangThai, maCB);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = result.Message });
        }

        [HttpPost("can-bo/doanh-nghiep/cap-nhat-trang-thai/{MaDN}")]
        public async Task<IActionResult> UpdateTrangThaiHopTac(string MaDN, HopTacDoanhNghiepStatus trangThai)
        {
            var maCB = User.Identity?.Name;
            var result = await _service.UpdateTrangThaiHopTac(MaDN, trangThai, maCB);
            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            } 
            return Json(new {success = true, message = result.Message});
        }

        [HttpGet("can-bo/thong-tin")]
        public async Task<IActionResult> ThongTinCanBo()
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
                return Unauthorized();

            var result = await _service.GetCanBoInfo(maDoanhNghiep);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy người đại diện";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpGet("can-bo/thong-tin/chinh-sua")]
        public async Task<IActionResult> ChinhSuaCanBo()
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
                return Unauthorized();

            var result = await _service.GetCanBoInfo(maDoanhNghiep);
            if (!result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result.Message ?? "Không tìm thấy người đại diện";
                return RedirectToAction(nameof(Index));
            }

            var data = new CanBoUpdateVM
            {
                HoTen = result.Data.HoTen ?? string.Empty,
                SoDienThoai = result.Data.SoDienThoai ?? string.Empty,
                Email = result.Data.Email ?? string.Empty,
                AnhNguoiDaiDien = result.Data.AnhNguoiDaiDien,
                BHTN = result.Data.BHTN,
                BHTT = result.Data.BHTT,
                STK = result.Data.STK
            };

            ViewBag.DoanhNghiep = await _service.GetDoanhNghiepByMaDN(maDoanhNghiep);
            return View(data);
        }

        [HttpPost("can-bo/thong-tin/chinh-sua")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSuaCanBo(CanBoUpdateVM model, IFormFile? AnhFile)
        {
            var maDoanhNghiep = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                var current = await _service.GetCanBoInfo(maDoanhNghiep);
                if (current.IsSuccess && current.Data != null)
                {
                    ViewBag.DoanhNghiep = await _service.GetDoanhNghiepByMaDN(maDoanhNghiep);
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
                    ViewBag.DoanhNghiep = await _service.GetDoanhNghiepByMaDN(maDoanhNghiep);
                    return View("NguoiDaiDienChinhSua", model);
                }

                model.AnhNguoiDaiDien = uploadResult.FilePath;
            }

            model.UpdatedBy = User.Identity?.Name;

            var result = await _service.UpdateCanBoInfo(maDoanhNghiep, model);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                ViewBag.DoanhNghiep = await _service.GetDoanhNghiepByMaDN(maDoanhNghiep);
                return View("NguoiDaiDienChinhSua", model);
            }

            if (!string.IsNullOrWhiteSpace(model.AnhNguoiDaiDien) && !string.Equals(oldAvatar, model.AnhNguoiDaiDien, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(oldAvatar))
            {
                await _fileStorage.DeleteAsync(oldAvatar);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(ThongTinCanBo));
        }
    }
}
