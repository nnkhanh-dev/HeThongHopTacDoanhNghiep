using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.Enums.NguoiDung;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopTacDoanhNghiep.Models
{
    public class DoanhNghiep
    {
        [Key]
        public string MaDN { get; set; }
        public string TenHienThi { get; set; }
        public string? Website { get; set; }
        public string? MaSoThue { get; set; }
        public string? TenPhapLy { get; set; }
        public string? Hotline { get; set; }
        public string? EmailCongTy { get; set; }
        public string? Logo { get; set; }
        public string? DiaChi { get; set; }
        public string? GioiThieu { get; set; }
        public int? QuyMoNhanSu { get; set; }
        public string MaNguoiDung { get; set; }
        public string NoiDungHopTac { get; set; }
        public HopTacDoanhNghiepStatus TrangThaiHopTac { get; set; } 
        public string? GhiChu { get; set; }
        [ForeignKey("MaNguoiDung")]
        public AppUser NguoiDung { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        public ICollection<TinTuyenDung> TinTuyenDungs { get; set; } = new List<TinTuyenDung>();
        public ICollection<HopTacDonVi> HopTacDonVis { get; set; } = new List<HopTacDonVi>();
    }
}
