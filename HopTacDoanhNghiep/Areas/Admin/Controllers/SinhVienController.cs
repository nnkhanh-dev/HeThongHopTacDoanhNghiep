using HopTacDoanhNghiep.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SinhVienController : Controller
    {
        private readonly ISinhVienAdmin _sinhVienAdmin;

        public SinhVienController(ISinhVienAdmin sinhVienAdmin)
        {
            _sinhVienAdmin = sinhVienAdmin;
        }

        [HttpGet("admin/sinh-vien")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _sinhVienAdmin.GetListSinhVien(pageIndex, pageSize, keyword);

            return View(result);
        }
        
        [HttpGet("admin/sinh-vien/{maSV}")]
        public async Task<IActionResult> Details(string maSV)
        {
            var result = await _sinhVienAdmin.GetSinhVienByMaSV(maSV);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return View(result);
        }
    }
}
