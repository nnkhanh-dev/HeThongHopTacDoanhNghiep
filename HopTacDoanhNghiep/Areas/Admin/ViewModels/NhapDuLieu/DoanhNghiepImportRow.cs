namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.NhapDuLieu
{
    public class DoanhNghiepImportRow
    {
        public int RowNumber { get; set; }
        public string TenPhapLy { get; set; }
        public string TenHienThi { get; set; }
        public string MaDN { get; set; }
        public string Website { get; set; }
        public string MaSoThue { get; set; }
        public string NgayThanhLapRaw { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string Logo { get; set; }
        public string GioiThieu { get; set; }
        public string QuyMoNhanSuRaw { get; set; }

        public bool IsValid { get; set; } = true;
        public string ErrorMessage { get; set; }
    }
}
