using HopTacDoanhNghiep.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<DanhMucBaiViet> DanhMucBaiViets { get; set; }
        public DbSet<BaiViet> BaiViets { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<DoanhNghiep> DoanhNghieps { get; set; }
        public DbSet<CanBo> CanBos { get; set; }
        public DbSet<TinTuyenDung> TinTuyenDungs { get; set; }
        public DbSet<DonUngTuyen> DonUngTuyens { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<DonVi> DonVis { get; set; }
        public DbSet<HopTacDonVi> HopTacDonVis { get; set; }
        public DbSet<LienHe> LienHes { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
