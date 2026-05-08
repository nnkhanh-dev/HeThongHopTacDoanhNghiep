using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.Nganh
{
    public class NganhEditVM
    {
        [Required(ErrorMessage = "Mã ngành không được để trống")]
        public string MaNganh { get; set; }
        [Required(ErrorMessage = "Tên ngành không được để trống")]
        public string TenNganh { get; set; }
        public string? TenChuyenNganh { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
