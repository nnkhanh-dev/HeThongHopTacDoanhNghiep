using Microsoft.AspNetCore.Identity;

namespace HopTacDoanhNghiep.Models
{
    public class AppUser : IdentityUser
    {
        public string HoTen { get; set; }
        public string? Avatar { get; set; }
    }
}
