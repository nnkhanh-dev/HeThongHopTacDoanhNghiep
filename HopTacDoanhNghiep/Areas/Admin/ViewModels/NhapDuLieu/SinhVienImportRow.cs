namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.NhapDuLieu
{
    public class SinhVienImportRow
    {
        public int RowNumber { get; set; }
        public string HoTen { get; set; }
        public string MaSV { get; set; }
        public string NgaySinhRaw { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }

        public bool IsValid { get; set; } = true;
        public string ErrorMessage { get; set; }
    }
}
