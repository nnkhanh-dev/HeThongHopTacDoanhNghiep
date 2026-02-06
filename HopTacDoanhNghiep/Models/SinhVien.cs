using HopTacDoanhNghiep.Enums.NguoiDung;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    [Index(nameof(MaSV), IsUnique = true)]
    public class SinhVien
    {
        [Key]
        public Guid Id { get; set; } 
        public string HoTen { get; set; }
        public string MaSV { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Lop { get; set; }
        public string Khoa { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string ChuyenNganh { get; set; }
        public string NguoiDungId { get; set; }
        public string? AnhThe { get; set; }
        public bool TimViec { get; set; } = false;
        public string? GhiChu { get; set; }
        [ForeignKey("NguoiDungId")]
        public AppUser NguoiDung { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
