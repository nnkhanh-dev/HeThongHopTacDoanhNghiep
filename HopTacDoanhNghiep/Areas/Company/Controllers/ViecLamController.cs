using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Company.ViewModels.ViecLam;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HopTacDoanhNghiep.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class ViecLamController : Controller
    {
        private readonly ITinTuyenDungCompany _viecLam;
        private readonly IFileStorage _fileStorage;

        public ViecLamController(ITinTuyenDungCompany viecLam, IFileStorage fileStorage)
        {
            _viecLam = viecLam;
            _fileStorage = fileStorage;
        }

        [HttpGet("doanh-nghiep/viec-lam")]
        public async Task<IActionResult> Index(
            int pageIndex = 1,
            int pageSize = 6,
            string? keyword = null,
            ViecLamStatus? status = null,
            ViecLamType? loaiViecLam = null,
            DoiTuongUngTuyen? doiTuongUngTuyen = null,
            TrinhDoType? trinhDo = null,
            long? luongMin = null,
            long? luongMax = null,
            bool? conHieuLuc = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null
        )
        {
            var doanhNghiepId = User.Identity?.Name;

            var result = await _viecLam.GetListTinTuyenDung(
                doanhNghiepId,
                pageIndex,
                pageSize,
                keyword,
                status,
                loaiViecLam,
                doiTuongUngTuyen,
                trinhDo,
                luongMin,
                luongMax,
                conHieuLuc,
                sapXepLuongToiDa,
                sapXepTheo
            );

            return View(result);
        }

        [HttpGet("doanh-nghiep/viec-lam/chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await _viecLam.GetTinTuyenDungById(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpGet("doanh-nghiep/viec-lam/tao-moi")]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost("doanh-nghiep/viec-lam/tao-moi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TinTuyenDungCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return View("Create", model);
            }

            var doanhNghiepId = User.Identity?.Name;
            model.CreatedBy = doanhNghiepId;
            model.MaDoanhNghiep = doanhNghiepId;

            var result = await _viecLam.CreateTinTuyenDung(model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View("Create", model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet("doanh-nghiep/viec-lam/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var viecLam = await _viecLam.GetTinTuyenDungById(id);

            if (!viecLam.IsSuccess)
            {
                TempData["ErrorMessage"] = viecLam.Message;
                return RedirectToAction("Index");
            }

            var data = new TinTuyenDungEditVM
            {
                TieuDe = viecLam.Data.TieuDe,
                MoTa = viecLam.Data.MoTa,
                YeuCau = viecLam.Data.YeuCau,
                UuTien = viecLam.Data.UuTien,
                QuyenLoi = viecLam.Data.QuyenLoi,
                LuongToiThieu = viecLam.Data.LuongToiThieu,
                LuongToiDa = viecLam.Data.LuongToiDa,
                DiaDiem = viecLam.Data.DiaDiem,
                TuKhoa = viecLam.Data.TuKhoa,
                NgayBatDau = viecLam.Data.NgayBatDau,
                NgayHetHan = viecLam.Data.NgayHetHan,
                LoaiViecLam = viecLam.Data.LoaiViecLam,
                DoiTuongUngTuyen = viecLam.Data.DoiTuongUngTuyen,
                TrinhDo = viecLam.Data.TrinhDo,
                Status = viecLam.Data.Status,
                MaDoanhNghiep = viecLam.Data.MaDoanhNghiep
            };

            return View(data);
        }

        [HttpPost("doanh-nghiep/viec-lam/chinh-sua/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TinTuyenDungEditVM model)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }

            var doanhNghiepId = User.Identity?.Name;
            model.UpdatedBy = doanhNghiepId;

            var result = await _viecLam.EditTinTuyenDung(id, model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpDelete("doanh-nghiep/viec-lam/xoa/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doanhNghiepId = User.Identity?.Name;

            var result = await _viecLam.DeleteTinTuyenDung(id, doanhNghiepId);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = result.Message });
        }

        [HttpPost("doanh-nghiep/viec-lam/upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            try
            {
                if (upload == null || upload.Length == 0)
                {
                    return Json(new
                    {
                        uploaded = 0,
                        error = new { message = "Không có file được chọn" }
                    });
                }

                var options = new FileUploadOptions
                {
                    Folder = "uploads/viec-lam",
                    AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
                    MaxSizeInBytes = 5 * 1024 * 1024, // 5MB
                    RenameFile = true
                };

                var uploadResult = await _fileStorage.UploadAsync(upload, options);

                if (!uploadResult.IsSuccess)
                {
                    return Json(new
                    {
                        uploaded = 0,
                        error = new { message = uploadResult.Message }
                    });
                }

                // CKEditor expects this format
                return Json(new
                {
                    uploaded = 1,
                    fileName = upload.FileName,
                    url = uploadResult.FilePath
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    uploaded = 0,
                    error = new { message = "Lỗi khi upload ảnh: " + ex.Message }
                });
            }
        }
    }
}
