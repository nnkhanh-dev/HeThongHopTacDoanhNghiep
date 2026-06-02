using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Officer.ViewModels
{
    public class NguoiDaiDienUpdateVM
    {
        [Required]
        public string HoTen { get; set; }

        [Required]
        [Phone]
        public string SoDienThoai { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // stored path to avatar image
        public string? AnhNguoiDaiDien { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
