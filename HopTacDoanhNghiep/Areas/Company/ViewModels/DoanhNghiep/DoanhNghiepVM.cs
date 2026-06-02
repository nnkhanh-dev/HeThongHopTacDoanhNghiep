using HopTacDoanhNghiep.Enums.HopTac;

namespace HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep
{
    public class DoanhNghiepVM
    {
        public string MaDN { get; set; } = string.Empty;
        public string TenHienThi { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? MaSoThue { get; set; }
        public string? TenPhapLy { get; set; }
        public string? Hotline { get; set; }
        public string? EmailCongTy { get; set; }
        public string? Logo { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiThieu { get; set; }
        public int? QuyMoNhanSu { get; set; }
        public string? NoiDungHopTac { get; set; }
        public HopTacDoanhNghiepStatus TrangThaiHopTac { get; set; }
        public string? GhiChu { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
