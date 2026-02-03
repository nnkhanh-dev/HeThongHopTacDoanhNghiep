using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet;
using HopTacDoanhNghiep.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DanhMucController : Controller
    {
        private readonly IDanhMucBaiVietAdmin _danhMuc;

        public DanhMucController(IDanhMucBaiVietAdmin danhMuc)
        {
            _danhMuc = danhMuc;            
        }

        [HttpGet("admin/danh-muc-bai-viet")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _danhMuc.GetListDanhMucBaiViet(pageIndex, pageSize, keyword);
            return View(result);
        }

        [HttpGet("admin/danh-muc-bai-viet/danh-sach")]
        public async Task<IActionResult> GetListDanhMuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _danhMuc.GetListDanhMucBaiViet(pageIndex, pageSize, keyword);
            return Json(new { success = true, data = result.Records, records = result.Records });
        }

        [HttpGet("admin/danh-muc-bai-viet/chi-tiet/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _danhMuc.GetDanhMucBaiVietById(id);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result);
        }

        [HttpGet("admin/danh-muc-bai-viet/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var detailResult = await  _danhMuc.GetDanhMucBaiVietById(id);

            if(!detailResult.IsSuccess)
            {
                TempData["ErrorMessage"] = detailResult.Message;
                return RedirectToAction("Index");
            }

            var data = new DanhMucBaiVietEditVM
            {
                Ten = detailResult.Data.Ten,
                MoTa = detailResult.Data.MoTa
            };

            return View(data);
        }

        [HttpGet("admin/danh-muc-bai-viet/tao-moi")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("admin/danh-muc-bai-viet/chinh-sua/{id}")]
        public async Task<IActionResult> Edit(int id, DanhMucBaiVietEditVM danhMuc)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return View(danhMuc);
            }

            var result = await _danhMuc.EditDanhMucBaiViet(id, danhMuc);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(danhMuc);
            }

            TempData["SuccessMessage"] = "Chỉnh sửa danh mục bài viết thành công";

            return RedirectToAction("Index");
        }

        [HttpPost("admin/danh-muc-bai-viet/tao-moi")]
        public async Task<IActionResult> Create(DanhMucBaiVietCreateVM danhMuc)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ";
                return View(danhMuc);
            }

            var result = await  _danhMuc.CreateDanhMucBaiViet(danhMuc);

            if(!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(danhMuc);
            }

            TempData["SuccessMessage"] = "Tạo mới danh mục bài viết thành công";
            
            return RedirectToAction("Index");
        }

        [HttpDelete("admin/danh-muc-bai-viet/xoa/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await  _danhMuc.DeleteDanhMucBaiViet(id);

            if(!result.IsSuccess)
            {
                return Json(new {success = false,  message = result.Message });
            }

            return Json(new { success = true, message = "Xóa danh mục bài viết thành công" });
        }
    }
}
