using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.ViewModels.DonVi;

namespace HopTacDoanhNghiep.Areas.Officer.ViewModels
{
    public class DangKyDoanhNghiepVM
    {
        public string MaDN { get; set; }
        public string TenHienThi { get; set; }
        public string? TenPhapLy { get; set; }
        public string? MaSoThue { get; set; }
        public string? Website { get; set; }
        public string? Hotline { get; set; }
        public string? EmailCongTy { get; set; }
        public string? NoiDungHopTac { get; set; }
        public HopTacDoanhNghiepStatus TrangThaiHopTac { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<DonViVM> SelectedDonVis { get; set; } = new();
    }
}
