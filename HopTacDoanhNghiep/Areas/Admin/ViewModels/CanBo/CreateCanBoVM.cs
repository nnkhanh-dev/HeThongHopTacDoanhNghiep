using HopTacDoanhNghiep.Enums.NguoiDung;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.CanBo
{
    public class CreateCanBoVM
    {
        [Required(ErrorMessage = "Đơn vị là bắt buộc")]
        public int? MaDV { get; set; }
        [Required(ErrorMessage = "Chức vụ là bắt buộc")]
        public int? MaCV { get; set; }
        public string? BHTT { get; set; }
        public string? BHTN { get; set; }
        public string? STK { get; set; }
        public string? AnhThe { get; set; }
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SDT { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        public NguoiDungStatus TrangThai { get; set; } = NguoiDungStatus.HoatDong;
    }
}
