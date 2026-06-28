using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HopTacDoanhNghiep.Controllers
{
    public class TinTuyenDungController : Controller
    {
        private readonly ITinTuyenDung _tinTuyenDung;

        public TinTuyenDungController(ITinTuyenDung tinTuyenDung)
        {
            _tinTuyenDung = tinTuyenDung;
        }

        [HttpGet("/viec-lam-tuyen-dung")]
        public async Task<IActionResult> Index(
            int pageIndex = 1,
            int pageSize = 10,
            string? keyword = null,
            string? loaiViecLam = null,
            string? trinhDo = null,
            string? doiTuongUngTuyen = null,
            double? luongMin = null,
            double? luongMax = null,
            bool? sapXepLuongToiDa = null,
            string? sapXepTheo = null
        )
        {
            ViecLamType? loaiViecLamEnum = null;
            TrinhDoType? trinhDoEnum = null;
            DoiTuongUngTuyen? doiTuongEnum = null;

            var slugToLoai = new Dictionary<string, ViecLamType>(System.StringComparer.OrdinalIgnoreCase)
            {
                { "ban-thoi-gian", ViecLamType.BanThoiGian },
                { "toan-thoi-gian", ViecLamType.ToanThoiGian },
                { "thuc-tap", ViecLamType.ThucTap },
                { "0", ViecLamType.BanThoiGian },
                { "1", ViecLamType.ToanThoiGian },
                { "2", ViecLamType.ThucTap }
            };

            var slugToTrinhDo = new Dictionary<string, TrinhDoType>(System.StringComparer.OrdinalIgnoreCase)
            {
                { "trung-cap", TrinhDoType.TrungCap },
                { "cao-dang", TrinhDoType.CaoDang },
                { "dai-hoc", TrinhDoType.DaiHoc },
                { "sau-dai-hoc", TrinhDoType.SauDaiHoc },
                { "1", TrinhDoType.TrungCap },
                { "2", TrinhDoType.CaoDang },
                { "3", TrinhDoType.DaiHoc },
                { "4", TrinhDoType.SauDaiHoc }
            };

            var slugToDoiTuong = new Dictionary<string, DoiTuongUngTuyen>(System.StringComparer.OrdinalIgnoreCase)
            {
                { "thuc-tap-sinh", DoiTuongUngTuyen.ThucTapSinh },
                { "sinh-vien-nam-cuoi", DoiTuongUngTuyen.SinhVienNamCuoi },
                { "moi-tot-nghiep", DoiTuongUngTuyen.MoiTotNghiep },
                { "da-tot-nghiep", DoiTuongUngTuyen.DaTotNghiep },
                { "1", DoiTuongUngTuyen.ThucTapSinh },
                { "2", DoiTuongUngTuyen.SinhVienNamCuoi },
                { "3", DoiTuongUngTuyen.MoiTotNghiep },
                { "4", DoiTuongUngTuyen.DaTotNghiep }
            };

            if (!string.IsNullOrWhiteSpace(loaiViecLam))
            {
                if (slugToLoai.TryGetValue(loaiViecLam.Trim(), out var tmp))
                    loaiViecLamEnum = tmp;
                else
                    return BadRequest();
            }

            if (!string.IsNullOrWhiteSpace(trinhDo))
            {
                if (slugToTrinhDo.TryGetValue(trinhDo.Trim(), out var tmp2))
                    trinhDoEnum = tmp2;
                else
                    return BadRequest();
            }

            if (!string.IsNullOrWhiteSpace(doiTuongUngTuyen))
            {
                if (slugToDoiTuong.TryGetValue(doiTuongUngTuyen.Trim(), out var tmp3))
                    doiTuongEnum = tmp3;
                else
                    return BadRequest();
            }

            var result = await _tinTuyenDung.GetListTinTuyenDung(pageIndex, pageSize, keyword, loaiViecLamEnum, trinhDoEnum, doiTuongEnum, luongMin, luongMax, sapXepLuongToiDa, sapXepTheo);
            return View(result);
        }

        [HttpGet("/viec-lam-tuyen-dung/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var result = await _tinTuyenDung.GetTinTuyenDungBySlug(slug);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            var relatedViecLam = await _tinTuyenDung.GetRelatedTinTuyenDung(1, 5, result.Data.Slug, null);
            result.Data.ViecLamLienQuan = relatedViecLam.Records.ToList();
            return View(result);
        }
    }
}
