using System.ComponentModel.DataAnnotations;

public class CanBoUpdateVM
{
    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string HoTen { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [RegularExpression(
        @"^0\d{9,10}$",
        ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có từ 10 đến 11 chữ số"
    )]
    public string SoDienThoai { get; set; }

    public string? BHTN { get; set; }
    public string? BHTT { get; set; }
    public string? STK { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    public string? AnhNguoiDaiDien { get; set; }

    public string? UpdatedBy { get; set; }
}