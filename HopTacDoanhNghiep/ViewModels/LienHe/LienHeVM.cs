using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.ViewModels.LienHe
{
    public class LienHeVM
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SDT { get; set; }
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string NoiDung { get; set; }
        [Required(ErrorMessage = "Mã xác nhận không được để trống")]
        public string CapCha { get; set; }
    }
}
