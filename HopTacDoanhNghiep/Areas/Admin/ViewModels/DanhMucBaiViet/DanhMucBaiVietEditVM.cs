using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.DanhMucBaiViet
{
    public class DanhMucBaiVietEditVM
    {
        [Required(ErrorMessage = "Tên danh mục bài viết không được để trống")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string MoTa { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
