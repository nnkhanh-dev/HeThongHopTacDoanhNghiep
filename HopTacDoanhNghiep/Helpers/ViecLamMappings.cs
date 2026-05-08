using HopTacDoanhNghiep.Enums.ViecLam;
using System.Collections.Generic;

namespace HopTacDoanhNghiep.Helpers
{
    public static class ViecLamMappings
    {
        private static readonly Dictionary<ViecLamType, string> _loaiToSlug = new()
        {
            { ViecLamType.BanThoiGian, "ban-thoi-gian" },
            { ViecLamType.ToanThoiGian, "toan-thoi-gian" },
            { ViecLamType.ThucTap, "thuc-tap" }
        };

        private static readonly Dictionary<string, ViecLamType> _slugToLoai = new(System.StringComparer.OrdinalIgnoreCase)
        {
            { "ban-thoi-gian", ViecLamType.BanThoiGian },
            { "toan-thoi-gian", ViecLamType.ToanThoiGian },
            { "thuc-tap", ViecLamType.ThucTap }
        };

        private static readonly Dictionary<DoiTuongUngTuyen, string> _doiTuongToSlug = new()
        {
            { DoiTuongUngTuyen.ThucTapSinh, "thuc-tap-sinh" },
            { DoiTuongUngTuyen.SinhVienNamCuoi, "sinh-vien-nam-cuoi" },
            { DoiTuongUngTuyen.MoiTotNghiep, "moi-tot-nghiep" },
            { DoiTuongUngTuyen.DaTotNghiep, "da-tot-nghiep" }
        };

        private static readonly Dictionary<string, DoiTuongUngTuyen> _slugToDoiTuong = new(System.StringComparer.OrdinalIgnoreCase)
        {
            { "thuc-tap-sinh", DoiTuongUngTuyen.ThucTapSinh },
            { "sinh-vien-nam-cuoi", DoiTuongUngTuyen.SinhVienNamCuoi },
            { "moi-tot-nghiep", DoiTuongUngTuyen.MoiTotNghiep },
            { "da-tot-nghiep", DoiTuongUngTuyen.DaTotNghiep }
        };

        private static readonly Dictionary<TrinhDoType, string> _trinhDoToSlug = new()
        {
            { TrinhDoType.TrungCap, "trung-cap" },
            { TrinhDoType.CaoDang, "cao-dang" },
            { TrinhDoType.DaiHoc, "dai-hoc" },
            { TrinhDoType.SauDaiHoc, "sau-dai-hoc" }
        };

        private static readonly Dictionary<string, TrinhDoType> _slugToTrinhDo = new(System.StringComparer.OrdinalIgnoreCase)
        {
            { "trung-cap", TrinhDoType.TrungCap },
            { "cao-dang", TrinhDoType.CaoDang },
            { "dai-hoc", TrinhDoType.DaiHoc },
            { "sau-dai-hoc", TrinhDoType.SauDaiHoc }
        };

        public static string ToSlug(this ViecLamType type) => _loaiToSlug[type];
        public static bool TryParseLoai(string? slug, out ViecLamType result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(slug)) return false;
            return _slugToLoai.TryGetValue(slug.Trim(), out result);
        }

        public static string ToSlug(this DoiTuongUngTuyen dt) => _doiTuongToSlug[dt];
        public static bool TryParseDoiTuong(string? slug, out DoiTuongUngTuyen result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(slug)) return false;
            return _slugToDoiTuong.TryGetValue(slug.Trim(), out result);
        }

        public static string ToSlug(this TrinhDoType td) => _trinhDoToSlug[td];
        public static bool TryParseTrinhDo(string? slug, out TrinhDoType result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(slug)) return false;
            return _slugToTrinhDo.TryGetValue(slug.Trim(), out result);
        }
    }
}
