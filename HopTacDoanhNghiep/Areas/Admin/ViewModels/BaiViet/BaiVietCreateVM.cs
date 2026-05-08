using HopTacDoanhNghiep.Enums;
using HopTacDoanhNghiep.Enums.BaiViet;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.BaiViet
{
    public class BaiVietCreateVM
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string TieuDe { get; set; }
        public IFormFile? AnhMinhHoa { get; set; }
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string NoiDung { get; set; }
        [Required(ErrorMessage = "Danh mục bài viết là bắt buộc")]
        public int? DanhMucId { get; set; }
        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public BaiVietStatus TrangThai { get; set; }
        public string? TacGia { get; set; } 
        public string? CreatedBy { get; set; }
        public string? TuKhoa { get; set; }
    }
}
