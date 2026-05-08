using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.Nganh
{
    public class NganhCreateVM
    {
        [Required(ErrorMessage = "Mã ngành không được để trống")]
        public string MaNganh { get; set; }
        [Required(ErrorMessage = "Tên ngành không được để trống")]
        public string TenNganh { get; set; }
        public string? TenChuyenNganh { get; set; }
        public string? CreatedBy { get; set; }
    }
}
