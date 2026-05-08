using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.Nganh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NganhController : Controller
    {
        private readonly INganhAdmin _nganh;

        public NganhController(INganhAdmin nganh)
        {
            _nganh = nganh;
        }

        [HttpGet("admin/nganh")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            var result = await _nganh.GetListNganh(pageIndex, pageSize, keyword);
            return View(result);
        }        

        [HttpGet("admin/nganh/chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _nganh.GetNganhById(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpGet("admin/nganh/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var detailResult = await _nganh.GetNganhById(id);

            if (!detailResult.IsSuccess)
            {
                TempData["ErrorMessage"] = detailResult.Message;
                return RedirectToAction("Index");
            }

            var data = new NganhEditVM
            {
                MaNganh = detailResult.Data.MaNganh,
                TenNganh = detailResult.Data.TenNganh,
                TenChuyenNganh = detailResult.Data.TenChuyenNganh
            };

            return View(data);
        }

        [HttpGet("admin/nganh/tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("admin/nganh/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id, NganhEditVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UpdatedBy = userId;
            var result = await _nganh.EditNganh(id, model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = "Chỉnh sửa ngành thành công";

            return RedirectToAction("Index");
        }

        [HttpPost("admin/nganh/tao-moi")]
        public async Task<IActionResult> Create(NganhCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.CreatedBy = userId;
            var result = await _nganh.CreateNganh(model);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = "Tạo mới ngành thành công";

            return RedirectToAction("Index");
        }

        [HttpDelete("admin/nganh/xoa/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _nganh.DeleteNganh(id, userId);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = "Xóa ngành thành công" });
        }
    }
}
