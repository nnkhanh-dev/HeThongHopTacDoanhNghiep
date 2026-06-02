using HopTacDoanhNghiep.Enums.HopTac;

namespace HopTacDoanhNghiep.Areas.Officer.ViewModels
{
    public class HopTacDonViVM
    {
        public int MaHTDV { get; set; }
        public string MaDN { get; set; }
        public string TenHienThi { get; set; }
        public string? TenPhapLy { get; set; }
        public string? MaSoThue { get; set; }
        public string? Website { get; set; }
        public string? Hotline { get; set; }
        public string? EmailCongTy { get; set; }
        public string? NoiDungHopTac { get; set; }
        public int? MaDV { get; set; }
        public string? TenDV { get; set; }
        public string? DonViTel { get; set; }
        public string? DonViEmail { get; set; }
        public string? DonViWebsite { get; set; }
        public HopTacDonViStatus TrangThaiHopTac { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
