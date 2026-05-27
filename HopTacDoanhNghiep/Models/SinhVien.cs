using HopTacDoanhNghiep.Enums.NguoiDung;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class SinhVien
    {
        [Key]
        public string MaSV { get; set; } 
        public string EmailGiaoDuc { get; set; }
        public string MaNguoiDung { get; set; }
        public string HoSoNangLuc { get; set; }
        public string? AnhThe { get; set; }
        public string? GhiChu { get; set; }
        [ForeignKey("MaNguoiDung")]
        public AppUser NguoiDung { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<DonUngTuyen> DonUngTuyens { get; set; }
    }
}
