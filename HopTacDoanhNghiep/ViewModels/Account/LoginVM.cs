using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Tài khoản là bắt buộc")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
