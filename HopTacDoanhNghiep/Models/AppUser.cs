using HopTacDoanhNghiep.Enums.NguoiDung;
using Microsoft.AspNetCore.Identity;

namespace HopTacDoanhNghiep.Models
{
    public class AppUser : IdentityUser
    {
        public string HoTen { get; set; }
        public string? Avatar { get; set; }
        public NguoiDungStatus TrangThai { get; set; } = NguoiDungStatus.HoatDong;
        public string? GhiChu { get; set; }
    }
}
