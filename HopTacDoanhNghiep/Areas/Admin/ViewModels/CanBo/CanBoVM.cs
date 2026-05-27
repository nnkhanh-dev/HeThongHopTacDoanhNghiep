using HopTacDoanhNghiep.Enums.NguoiDung;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.CanBo
{
    public class CanBoVM
    {
        public string MaCB { get; set; }
        public int MaDV { get; set; }
        public string? TenDonVi { get; set; }
        public int MaCV { get; set; }
        public string? TenChucVu { get; set; }
        public string BHTT { get; set; }
        public string BHTN { get; set; }
        public string STK { get; set; }
        public string AnhThe { get; set; }
        public string HoTen { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public NguoiDungStatus TrangThai { get; set; }
    }
}
