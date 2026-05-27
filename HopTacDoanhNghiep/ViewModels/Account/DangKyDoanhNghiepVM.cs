using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HopTacDoanhNghiep.ViewModels.DonVi;

namespace HopTacDoanhNghiep.ViewModels.Account
{
    public class DangKyDoanhNghiepVM
    {
        // Thông tin người đại diện
        [Required(ErrorMessage = "Họ tên người đại diện là bắt buộc")]
        public string HoTenNguoiDaiDien { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoaiNguoiDaiDien { get; set; }

        [Required(ErrorMessage = "Email người đại diện là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string EmailNguoiDaiDien { get; set; }

        // Thông tin doanh nghiệp
        [Required(ErrorMessage = "Tên hiển thị doanh nghiệp là bắt buộc")]
        public string TenHienThiDoanhNghiep { get; set; }
        [Required(ErrorMessage = "Tên pháp lý doanh nghiệp là bắt buộc")]

        public string TenPhapLyDoanhNghiep { get; set; }

        [Required(ErrorMessage = "Mã số thuế là bắt buộc")]
        public string MaSoThue { get; set; }
        public string? Website { get; set; }

        [Required(ErrorMessage = "Hotline là bắt buộc")]
        public string Hotline { get; set; }

        [Required(ErrorMessage = "Email công ty là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email công ty không hợp lệ")]
        public string EmailCongTy { get; set; }

        [Required(ErrorMessage = "Nội dung hợp tác là bắt buộc")]
        public string NoiDungHopTac { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một đơn vị hợp tác")]
        public List<int>? SelectedDonViIds { get; set; } = new();
        public List<DonViVM> SelectedDonVis { get; set; } = new();
    }
}
