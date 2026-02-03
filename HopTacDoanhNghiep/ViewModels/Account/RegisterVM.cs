using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Tài khoản là bắt buộc")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Mật khẩu phải gồm chữ hoa, chữ thường, số và ký tự đặc biệt")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại phải có 10–11 chữ số và bắt đầu bằng số 0")]
        public string PhoneNumber { get; set; }
    }
}
