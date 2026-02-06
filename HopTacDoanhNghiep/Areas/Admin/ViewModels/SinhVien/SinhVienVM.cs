namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.SinhVien
{
    public class SinhVienVM
    {
        public Guid? Id { get; set; }
        public string? HoTen { get; set; }
        public string? MaSV { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? Lop { get; set; }
        public string? Khoa { get; set; }
        public string? Email { get; set; }
        public string? SDT { get; set; }
        public string? ChuyenNganh { get; set; }
        public string? AnhThe { get; set; }
        public bool? TimViec { get; set; } = false;
        public string? GhiChu { get; set; }
    }
}
