using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.BaiViet;
using HopTacDoanhNghiep.Enums;
using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BaiVietController : Controller
    {
        private readonly IBaiVietAdmin _baiViet;
        private readonly IFileStorage _fileStorage;

        public BaiVietController(IBaiVietAdmin baiViet, IFileStorage fileStorage)
        {
            _baiViet = baiViet;
            _fileStorage = fileStorage;
        }

        [HttpGet("admin/bai-viet")]
        public async Task<IActionResult> Index(
            int pageIndex = 1,
            int pageSize = 10,
            string? keyword = null,
            int? danhMucId = null,
            BaiVietStatus? status = null
        )
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, danhMucId, status);
            return View(result);
        }

        [HttpGet("admin/bai-viet/chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var baiViet = await _baiViet.GetBaiVietById(id);
            if (!baiViet.IsSuccess)
            {
                TempData["ErrorMessage"] = baiViet.Message;
                return RedirectToAction("Index");
            }
            return View(baiViet);
        }

        [HttpGet("admin/bai-viet/tao-moi")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost("admin/bai-viet/tao-moi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaiVietCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ!";
                return View(model);
            }

            model.TacGia = User.FindFirst("FullName")?.Value ?? "Admin";

            var result = await _baiViet.CreateBaiViet(model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet("admin/bai-viet/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            var baiViet = await _baiViet.GetBaiVietById(id);

            if (!baiViet.IsSuccess)
            {
                TempData["ErrorMessage"] = baiViet.Message;
                return RedirectToAction("Index");
            }

            var data = new BaiVietEditVM
            {
                TieuDe = baiViet.Data.TieuDe,
                NoiDung = baiViet.Data.NoiDung,
                AnhHienTai = baiViet.Data.AnhMinhHoa,
                DanhMucId = baiViet.Data.DanhMucId ?? 0,
                DanhMuc = baiViet.Data.DanhMuc ?? "",
                TrangThai = baiViet.Data.TrangThai ?? BaiVietStatus.Nhap,
            };

            return View(data);
        }

        [HttpPost("admin/bai-viet/chinh-sua/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BaiVietEditVM model)
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

            var result = await _baiViet.EditBaiViet(id, model);

            if(!result.IsSuccess){
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpDelete("admin/bai-viet/xoa/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _baiViet.DeleteBaiViet(id);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = result.Message });
        }

        [HttpPost("admin/bai-viet/upload-image")]
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
                    Folder = "uploads/bai-viet",
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
