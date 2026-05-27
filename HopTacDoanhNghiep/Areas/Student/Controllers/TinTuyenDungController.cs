using HopTacDoanhNghiep.Areas.Student.Services;
using HopTacDoanhNghiep.Enums.ViecLam;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class TinTuyenDungController : Controller
    {
        private readonly ITinTuyenDungStudent _tinTuyenDung;

        public TinTuyenDungController(ITinTuyenDungStudent tinTuyenDung)
        {
            _tinTuyenDung = tinTuyenDung;
        }

        [HttpGet("sinh-vien/tin-tuyen-dung")]
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

            var result = await _tinTuyenDung.GetListTinTuyenDung(
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

        [HttpGet("/sinh-vien/tin-tuyen-dung/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return BadRequest();
            }

            var chiTiet = await _tinTuyenDung.GetTinTuyenDungBySlug(slug);

            if (chiTiet.IsSuccess == false)
            {
                return NotFound();
            }

            return View(chiTiet);
        }
    }
}
