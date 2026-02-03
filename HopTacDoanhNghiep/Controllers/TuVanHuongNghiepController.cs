using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Controllers
{
    public class TuVanHuongNghiepController : Controller
    {
        private readonly IBaiViet _baiViet;

        public TuVanHuongNghiepController(IBaiViet baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/tu-van-huong-nghiep")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/tu-van-huong-nghiep/cam-nang")]
        public async Task<IActionResult> CamNang(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "cam-nang");
            return View(result);
        }

        [HttpGet("/tu-van-huong-nghiep/ute-khoi-nghiep")]
        public async Task<IActionResult> UTEKhoiNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "ute-khoi-nghiep");
            return View(result);
        }

        [HttpGet("/tu-van-huong-nghiep/phat-trien-nghe-nghiep")]
        public async Task<IActionResult> PhatTrienNgheNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "phat-trien-nghe-nghiep");
            return View(result);
        }

        [HttpGet("/tu-van-huong-nghiep/huan-luyen-ky-nang")]
        public async Task<IActionResult> HuanLuyenKyNang(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "huan-luyen-ky-nang");
            return View(result);
        }

        [HttpGet("/tu-van-huong-nghiep/cam-nang/{slug}")]
        public async Task<IActionResult> BaiVietCamNang(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        [HttpGet("/tu-van-huong-nghiep/ute-khoi-nghiep/{slug}")]
        public async Task<IActionResult> BaiVietUTEKhoiNghiep(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        [HttpGet("/tu-van-huong-nghiep/phat-trien-nghe-nghiep/{slug}")]
        public async Task<IActionResult> BaiVietPhatTrienNgheNghiep(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        [HttpGet("/tu-van-huong-nghiep/huan-luyen-ky-nang/{slug}")]
        public async Task<IActionResult> BaiVietHuanLuyenKyNang(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }
    }
}
