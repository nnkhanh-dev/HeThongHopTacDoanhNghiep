using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Controllers
{
    public class ChuongTrinhSuKienController : Controller
    {
        private readonly IBaiViet _baiViet;

        public ChuongTrinhSuKienController(IBaiViet baiViet)
        {
            _baiViet = baiViet;
        }

        [HttpGet("/chuong-trinh-su-kien")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/chuong-trinh-su-kien/ngay-hoi-tuyen-dung")]
        public async Task<IActionResult> NgayHoiTuyenDung(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "ngay-hoi-tuyen-dung");
            return View(result);
        }

        [HttpGet("/chuong-trinh-su-kien/cuoc-thi-sinh-vien")]
        public async Task<IActionResult> CuocThiSinhVien(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "cuoc-thi-sinh-vien");
            return View(result);
        }

        [HttpGet("/chuong-trinh-su-kien/hoi-thao-giao-luu")]
        public async Task<IActionResult> HoiThaoGiaoLuu(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _baiViet.GetListBaiViet(pageIndex, pageSize, keyword, "hoi-thao-giao-luu");
            return View(result);
        }

        [HttpGet("/chuong-trinh-su-kien/ngay-hoi-tuyen-dung/{slug}")]
        public async Task<IActionResult> BaiVietNgayHoiTuyenDung(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }

        [HttpGet("/chuong-trinh-su-kien/cuoc-thi-sinh-vien/{slug}")]
        public async Task<IActionResult> BaiVietCuocThiSinhVien(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }

        [HttpGet("/chuong-trinh-su-kien/hoi-thao-giao-luu/{slug}")]
        public async Task<IActionResult> BaiVietHoiThaoGiaoLuu(string slug)
        {
            var baiViet = await _baiViet.GetBaiVietBySlug(slug);

            if (!baiViet.IsSuccess)
            {
                return NotFound();
            }
            var relatedBaiViet = await _baiViet.GetListRelatedBaiViet(1, 10, baiViet.Data.Slug, null, baiViet.Data.DanhMucSlug);
            baiViet.Data.BaiVietLienQuan = relatedBaiViet.Records.ToList();
            return View(baiViet);
        }
    }
}
