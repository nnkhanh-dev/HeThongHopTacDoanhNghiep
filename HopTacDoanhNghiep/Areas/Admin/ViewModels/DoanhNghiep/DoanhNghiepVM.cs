using HopTacDoanhNghiep.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Areas.Admin.ViewModels.DoanhNghiep
{
    public class DoanhNghiepVM
    {
        public Guid? Id { get; set; }
        public string? MaDN { get; set; }
        public string? TenHienThi { get; set; }
        public string? Website { get; set; }
        public string? MaSoThue { get; set; }
        public DateTime? NgayThanhLap { get; set; }
        public string? TenPhapLy { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? Logo { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiThieu { get; set; }
        public int? QuyMoNhanSu { get; set; }
        public string? GhiChu { get; set; }
    }
}
