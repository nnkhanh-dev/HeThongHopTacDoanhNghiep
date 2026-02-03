using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet
{
    public class DanhMucBaiVietCreateVM
    {
        [Required(ErrorMessage = "Tên danh mục bài viết không được để trống")]
        public string Ten { get; set; }
        public string? MoTa { get; set; }
    }
}
