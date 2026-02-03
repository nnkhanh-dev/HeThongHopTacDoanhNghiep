using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Controllers
{
    public class GioiThieuController : Controller
    {
        [HttpGet("/gioi-thieu")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/gioi-thieu/gioi-thieu-chung")]
        public IActionResult GioiThieuChung()
        {
            return View();
        }

        [HttpGet("/gioi-thieu/qua-trinh-hinh-thanh-phat-trien")]
        public IActionResult QuaTrinhPhatTrien()
        {
            return View();
        }

        [HttpGet("/gioi-thieu/chuc-nang-nhiem-vu")]
        public IActionResult ChucNangNhiemVu()
        {
            return View();
        }

        [HttpGet("/gioi-thieu/doi-ngu-can-bo")]
        public IActionResult DoiNguCanBo()
        {
            return View();
        }
    }
}
