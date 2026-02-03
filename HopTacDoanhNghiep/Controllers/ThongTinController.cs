using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HopTacDoanhNghiep.Controllers
{
    public class ThongTinController : Controller
    {
        private readonly IBaiViet _baiViet;

        public ThongTinController(IBaiViet baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/thong-tin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/thong-tin/tin-tuc")]
        public async Task<IActionResult> TinTuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "tin-tuc");
            return View(result);
        }

        [HttpGet("/thong-tin/thong-bao")]
        public async Task<IActionResult> ThongBao(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "thong-bao");
            return View(result);
        }

        [HttpGet("/thong-tin/ute-doanh-nghiep")]
        public async Task<IActionResult> UTEDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "ute-doanh-nghiep");
            return View(result);
        }

        [HttpGet("/thong-tin/hop-tac-doanh-nghiep")]
        public async Task<IActionResult> HopTacDoanhNghiep(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "hop-tac-doanh-nghiep");
            return View(result);
        }

        [HttpGet("/thong-tin/tin-tuc/{slug}")]
        public async Task<IActionResult> BaiVietTinTuc(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        [HttpGet("/thong-tin/thong-bao/{slug}")]
        public async Task<IActionResult> BaiVietThongBao(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        [HttpGet("/thong-tin/ute-doanh-nghiep/{slug}")]
        public async Task<IActionResult> BaiVietUTEDoanhNghiep(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }

            return View(baiViet);
        }

        [HttpGet("/thong-tin/hop-tac-doanh-nghiep/{slug}")]
        public async Task<IActionResult> BaiVietHopTacDoanhNghiep(string slug)
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
