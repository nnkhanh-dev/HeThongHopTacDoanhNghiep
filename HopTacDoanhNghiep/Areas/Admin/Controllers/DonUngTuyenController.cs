using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Enums.HoSo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonUngTuyenController : Controller
    {
        private readonly IDonUngTuyenAdmin _donUngTuyen;

        public DonUngTuyenController(IDonUngTuyenAdmin donUngTuyenAdmin)
        {
            _donUngTuyen = donUngTuyenAdmin;
        }

        [HttpGet("admin/don-ung-tuyen")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 6, string? keyword = null, HoSoStatus? hoSoStatus = null, int? maTTD = null)
        {
            var result = await _donUngTuyen.GetListDonUngTuyen(pageIndex, pageSize, keyword, hoSoStatus, maTTD);
            return View(result);
        }

       
    }
}
