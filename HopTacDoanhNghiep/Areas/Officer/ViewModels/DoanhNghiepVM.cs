using HopTacDoanhNghiep.Enums.HopTac;
using System;
using System.ComponentModel.DataAnnotations;

namespace HopTacDoanhNghiep.Areas.Officer.ViewModels
{
    public class DoanhNghiepVM
    {   // Thông tin doanh nghiệp
        public string MaDN { get; set; }
        public string TenHienThi { get; set; }
        public string? TenPhapLy { get; set; }
        public string? MaSoThue { get; set; }
        public string? Website { get; set; }
        public string? Hotline { get; set; }
        public string? EmailCongTy { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiThieu { get; set; }
        public HopTacDoanhNghiepStatus TrangThaiHopTac { get; set; }
        public int? QuyMoNhanSu { get; set; }
        public string? NoiDungHopTac { get; set; }

        // Thông tin người đại diện
        public string HoTenNguoiDaiDien { get; set; }
        public string SoDienThoaiNguoiDaiDien { get; set; }
        public string EmailNguoiDaiDien { get; set; }
        public string AnhNguoiDaiDien { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
