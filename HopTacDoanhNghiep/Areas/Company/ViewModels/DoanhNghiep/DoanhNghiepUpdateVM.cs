using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Company.ViewModels.DoanhNghiep
{
    public class DoanhNghiepUpdateVM
    {
        [Required(ErrorMessage = "Tên hiển thị là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên hiển thị không được vượt quá 200 ký tự")]
        public string TenHienThi { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Website không được vượt quá 500 ký tự")]
        public string? Website { get; set; }

        [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
        public string? MaSoThue { get; set; }

        [StringLength(200, ErrorMessage = "Tên pháp lý không được vượt quá 200 ký tự")]
        public string? TenPhapLy { get; set; }

        [StringLength(50, ErrorMessage = "Hotline không được vượt quá 50 ký tự")]
        public string? Hotline { get; set; }

        [Required(ErrorMessage = "Email công ty là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email công ty không hợp lệ")]
        [StringLength(200, ErrorMessage = "Email công ty không được vượt quá 200 ký tự")]
        public string EmailCongTy { get; set; } = string.Empty;

        public string? Logo { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string? DiaChi { get; set; }

        [StringLength(4000, ErrorMessage = "Giới thiệu không được vượt quá 4000 ký tự")]
        public string? GioiThieu { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quy mô nhân sự phải là số không âm")]
        public int? QuyMoNhanSu { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
