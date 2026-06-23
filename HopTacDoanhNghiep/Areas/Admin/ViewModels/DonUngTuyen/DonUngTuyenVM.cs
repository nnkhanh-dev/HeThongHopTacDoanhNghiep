using HopTacDoanhNghiep.Enums.HoSo;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.DonUngTuyen
{
    public class DonUngTuyenVM
    {
        public int MaUT { get; set; }
        public string? MaSV { get; set; }
        public string? TenSinhVien { get; set; }
        public int? MaTTD { get; set; }
        public string? TieuDeTinTuyenDung { get; set; }
        public string? MaDoanhNghiep { get; set; }
        public string? TenDoanhNghiep { get; set; }
        public string? HoSoUngTuyen { get; set; }
        public HoSoStatus TrangThai { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
