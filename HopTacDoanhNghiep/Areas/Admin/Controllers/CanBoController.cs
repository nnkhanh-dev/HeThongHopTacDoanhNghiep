using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.CanBo;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CanBoController : Controller
    {
        private readonly ICanBoAdmin _canBoAdmin;
        private readonly IDonViAdmin _donViAdmin;
        private readonly IChucVuAdmin _chucVuAdmin;
        private readonly IFileStorage _fileStorage;

        public CanBoController(ICanBoAdmin canBoAdmin, IDonViAdmin donViAdmin, IChucVuAdmin chucVuAdmin, IFileStorage fileStorage)
        {
            _canBoAdmin = canBoAdmin;
            _donViAdmin = donViAdmin;
            _chucVuAdmin = chucVuAdmin;
            _fileStorage = fileStorage;
        }

        private async Task LoadFormDataAsync()
        {
            var donVis = await _donViAdmin.GetDonViAsync(1, 1000, null);
            var chucVus = await _chucVuAdmin.GetChucVuAsync(1, 1000, null);

            ViewBag.DonVis = new SelectList(donVis.Records, nameof(HopTacDoanhNghiep.Areas.Admin.ViewModels.DonVi.DonViVM.MaDonVi), nameof(HopTacDoanhNghiep.Areas.Admin.ViewModels.DonVi.DonViVM.TenDonVi));
            ViewBag.ChucVus = new SelectList(chucVus.Records, nameof(HopTacDoanhNghiep.Areas.Admin.ViewModels.ChucVu.ChucVuVM.maChucVu), nameof(HopTacDoanhNghiep.Areas.Admin.ViewModels.ChucVu.ChucVuVM.tenChucVu));
        }

        [HttpGet("admin/can-bo")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _canBoAdmin.GetListCanBo(pageIndex, pageSize, keyword);

            return View(result);
        }

        [HttpGet("admin/can-bo/them-moi")]
        public async Task<IActionResult> Create()
        {
            await LoadFormDataAsync();
            return View();
        }

        [HttpPost("admin/can-bo/them-moi")]
        public async Task<IActionResult> Create(CreateCanBoVM model, IFormFile? AnhTheFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                await LoadFormDataAsync();
                return View(model);
            }

            if (AnhTheFile != null && AnhTheFile.Length > 0)
            {
                var uploadResult = await _fileStorage.UploadAsync(
                    AnhTheFile,
                    new FileUploadOptions
                    {
                        Folder = "uploads/can-bo",
                        AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
                        MaxSizeInBytes = 5 * 1024 * 1024,
                        RenameFile = true
                    });

                if (!uploadResult.IsSuccess)
                {
                    TempData["ErrorMessage"] = "Upload ảnh thẻ thất bại: " + uploadResult.Message;
                    await LoadFormDataAsync();
                    return View(model);
                }

                model.AnhThe = uploadResult.FilePath;
            }

            var createdBy = User?.Identity?.Name;
            var result = await _canBoAdmin.CreateCanBo(model, createdBy);
           
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Thêm cán bộ thất bại";
                await LoadFormDataAsync();
                return View(model);
            }

            TempData["SuccessMessage"] = "Thêm cán bộ thành công";
            return RedirectToAction("Index");
        }

        [HttpGet("admin/can-bo/chi-tiet/{maCanBo}")]
        public async Task<IActionResult> ChiTiet(string maCanBo)
        {
            var result = await _canBoAdmin.GetCanBoByMaCB(maCanBo);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Không tìm thấy cán bộ";
                return RedirectToAction("Index");
            }
            return View(result.Data);
        }

        [HttpGet("admin/can-bo/chinh-sua/{maCanBo}")]
        public async Task<IActionResult> Edit(string maCanBo)
        {
            var result = await _canBoAdmin.GetCanBoByMaCB(maCanBo);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Không tìm thấy cán bộ";
                return RedirectToAction("Index");
            }

            var editModel = new EditCanBoVM
            {
                MaCB = result.Data.MaCB,
                MaDV = result.Data.MaDV,
                MaCV = result.Data.MaCV,
                BHTT = result.Data.BHTT,
                BHTN = result.Data.BHTN,
                STK = result.Data.STK,
                AnhThe = result.Data.AnhThe,
                HoTen = result.Data.HoTen,
                SDT = result.Data.SDT,
                Email = result.Data.Email,
                TrangThai = result.Data.TrangThai
            };

            await LoadFormDataAsync();

            return View(editModel);
        }

        [HttpPost("admin/can-bo/chinh-sua/{maCanBo}")]
        public async Task<IActionResult> Edit(string maCanBo, EditCanBoVM model, IFormFile? AnhTheFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                await LoadFormDataAsync();
                return View(model);
            }

            var currentImagePath = model.AnhThe;

            if (AnhTheFile != null && AnhTheFile.Length > 0)
            {
                var uploadResult = await _fileStorage.UploadAsync(
                    AnhTheFile,
                    new FileUploadOptions
                    {
                        Folder = "uploads/can-bo",
                        AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
                        MaxSizeInBytes = 5 * 1024 * 1024,
                        RenameFile = true
                    });

                if (!uploadResult.IsSuccess)
                {
                    TempData["ErrorMessage"] = "Upload ảnh thẻ thất bại: " + uploadResult.Message;
                    await LoadFormDataAsync();
                    return View(model);
                }

                model.AnhThe = uploadResult.FilePath;

                if (!string.IsNullOrWhiteSpace(currentImagePath) && !string.Equals(currentImagePath, model.AnhThe, System.StringComparison.OrdinalIgnoreCase))
                {
                    await _fileStorage.DeleteAsync(currentImagePath);
                }
            }

            var updatedBy = User?.Identity?.Name ?? "system";

            var result = await _canBoAdmin.EditCanBo(maCanBo, model, updatedBy);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = "Chỉnh sửa cán bộ thất bại";
                await LoadFormDataAsync();
                return View(model);
            }

            TempData["SuccessMessage"] = "Chỉnh sửa cán bộ thành công";

            return RedirectToAction("Index");
        }


    }
}
