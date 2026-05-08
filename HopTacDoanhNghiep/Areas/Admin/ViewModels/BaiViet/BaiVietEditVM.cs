using HopTacDoanhNghiep.Enums.BaiViet;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.BaiViet
{
    public class BaiVietEditVM
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string TieuDe { get; set; }

        public IFormFile? AnhMinhHoa { get; set; }
        public string? AnhHienTai { get; set; }
        public string? DanhMuc { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Danh mục bài viết là bắt buộc")]
        public int DanhMucId { get; set; }
        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public BaiVietStatus TrangThai { get; set; }
        public string? UpdatedBy { get; set; }
        public string? TuKhoa { get; set; }
    }
}
