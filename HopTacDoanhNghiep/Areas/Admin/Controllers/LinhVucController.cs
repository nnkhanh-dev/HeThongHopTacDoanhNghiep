using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.LinhVuc;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.Nganh;
using HopTacDoanhNghiep.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LinhVucController : Controller
    {
        private readonly ILinhVucAdmin _linhVuc;

        public LinhVucController(ILinhVucAdmin linhVuc)
        {
            _linhVuc = linhVuc;
        }

        [HttpGet("admin/linh-vuc")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            var result = await _linhVuc.GetListLinhVuc(pageIndex, pageSize, keyword);
            return View(result);
        }

        [HttpGet("admin/linh-vuc/chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _linhVuc.GetLinhVucById(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpGet("admin/linh-vuc/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var detailResult = await _linhVuc.GetLinhVucById(id);

            if (!detailResult.IsSuccess)
            {
                TempData["ErrorMessage"] = detailResult.Message;
                return RedirectToAction("Index");
            }

            var data = new LinhVucEditVM
            {
                Ten = detailResult.Data.Ten,
                MoTa = detailResult.Data.MoTa
            };

            return View(data);
        }

        [HttpGet("admin/linh-vuc/tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("admin/linh-vuc/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id, LinhVucEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UpdatedBy = userId;
            var result = await _linhVuc.EditLinhVuc(id, model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = "Chỉnh sửa lĩnh vực thành công";

            return RedirectToAction("Index");
        }

        [HttpPost("admin/linh-vuc/tao-moi")]
        public async Task<IActionResult> Create(LinhVucCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.CreatedBy = userId;
            var result = await _linhVuc.CreateLinhVuc(model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = "Tạo mới lĩnh vực thành công";

            return RedirectToAction("Index");
        }

        [HttpDelete("admin/linh-vuc/xoa/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _linhVuc.DeleteLinhVuc(id, userId);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = "Xóa lĩnh vực thành công" });
        }
    }
}
