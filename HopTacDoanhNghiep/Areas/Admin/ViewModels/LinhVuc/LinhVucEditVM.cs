using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.LinhVuc
{
    public class LinhVucEditVM
    {
        [Required(ErrorMessage = "Tên lĩnh vực không được để trống")]
        public string Ten { get; set; }
        public string? MoTa { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
