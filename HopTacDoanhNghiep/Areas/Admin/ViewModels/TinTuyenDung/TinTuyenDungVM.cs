using HopTacDoanhNghiep.Enums.ViecLam;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.TinTuyenDung
{
    public class TinTuyenDungVM
    {
        public int? MaTTD { get; set; }
        public string? TieuDe { get; set; }
        public string? Slug { get; set; }
        public string? MoTa { get; set; }
        public string? YeuCau { get; set; }
        public string? UuTien { get; set; }
        public string? QuyenLoi { get; set; }
        public decimal? LuongToiThieu { get; set; }
        public decimal? LuongToiDa { get; set; }
        public string? DiaDiem { get; set; }
        public string? TuKhoa { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public ViecLamType? LoaiViecLam { get; set; }
        public DoiTuongUngTuyen? DoiTuongUngTuyen { get; set; }
        public TrinhDoType? TrinhDo { get; set; }
        public ViecLamStatus? Status { get; set; }
        public string? MaDoanhNghiep { get; set; }
        public string? DoanhNghiep { get; set; }
        public string? LogoDoanhNghiep { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
