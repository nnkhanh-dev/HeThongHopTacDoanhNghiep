using HopTacDoanhNghiep.Enums.NguoiDung;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    [Index(nameof(MaDN), IsUnique = true)]
    public class DoanhNghiep
    {
        [Key]
        public Guid Id { get; set; }
        public string MaDN { get; set; }
        public string TenHienThi { get; set; }
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
        public string NguoiDungId { get; set; }
        public NguoiDungStatus TrangThai { get; set; } = NguoiDungStatus.HoatDong;
        public string? GhiChu { get; set; }
        [ForeignKey("NguoiDungId")]
        public AppUser NguoiDung { get; set; }

    }
}
