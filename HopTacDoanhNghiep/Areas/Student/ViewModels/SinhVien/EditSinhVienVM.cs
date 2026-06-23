using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Student.ViewModels.SinhVien
{
    public class EditSinhVienVM
    {
        [Url]
        public string? HoSoNangLuc { get; set; }
        public string? AnhThe { get; set; }
        public string? Email { get; set; }
        public string? SDT { get; set; }
    }
}
