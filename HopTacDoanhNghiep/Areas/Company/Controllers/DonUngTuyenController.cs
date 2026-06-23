using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Enums.HoSo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class DonUngTuyenController : Controller
    {
        private readonly IDonUngTuyenCompany _donUngTuyen;

        public DonUngTuyenController(IDonUngTuyenCompany donUngTuyen)
        {
            _donUngTuyen = donUngTuyen;
        }

        [HttpGet("doanh-nghiep/don-ung-tuyen")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 6, string? keyword = null, HoSoStatus? hoSoStatus = null, int? maTTD = null)
        {
            var maDoanhNghiep = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
            {
                return Unauthorized();
            }

            var result = await _donUngTuyen.GetListDonUngTuyen(pageIndex, pageSize, keyword, maDoanhNghiep, hoSoStatus, maTTD);
            return View(result);
        }

        [HttpPost("doanh-nghiep/don-ung-tuyen/thay-doi-trang-thai")]
        public async Task<IActionResult> ChangeStatus(int maUT, HopTacDoanhNghiep.Enums.HoSo.HoSoStatus trangThai)
        {
            var maDoanhNghiep = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(maDoanhNghiep))
            {
                return Json(new { success = false, message = "Unauthorized" });
            }

            var result = await _donUngTuyen.UpdateTrangThaiDonUngTuyen(maUT, trangThai, maDoanhNghiep);

            if (!result.IsSuccess)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = result.Message });
        }
    }
}
