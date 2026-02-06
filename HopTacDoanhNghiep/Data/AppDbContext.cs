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
        public DbSet<LichSuNhapDuLieu> LichSuNhapDuLieus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cấu hình mối quan hệ 1-1 giữa AppUser và SinhVien
            builder.Entity<SinhVien>()
                .HasOne(sv => sv.NguoiDung)
                .WithOne()
                .HasForeignKey<SinhVien>(sv => sv.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình mối quan hệ 1-1 giữa AppUser và DoanhNghiep
            builder.Entity<DoanhNghiep>()
                .HasOne(dn => dn.NguoiDung)
                .WithOne()
                .HasForeignKey<DoanhNghiep>(dn => dn.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
