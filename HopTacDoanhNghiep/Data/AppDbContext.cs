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
        public DbSet<Nganh> Nganhs { get; set; }
        public DbSet<LinhVuc> LinhVucs { get; set; }
        public DbSet<ViecLam> ViecLams { get; set; }
        public DbSet<LichPhongVan> LichPhongVans { get; set; }
        public DbSet<SinhVienViecLam> SinhVienViecLams { get; set; }
        // Bảng trung gian nhiều-nhiều
        public DbSet<LinhVucNganh> LinhVucNganhs { get; set; }
        public DbSet<LuuTru> LuuTrus { get; set; }
        public DbSet<DangKyPhongVan> DangKyPhongVans { get; set; }
        public DbSet<LienHe> LienHes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ===== Cấu hình Unique Index với Soft Delete =====
            
            // MaSV unique chỉ khi DeletedAt IS NULL (cho phép tái sử dụng mã sau khi xóa)
            builder.Entity<SinhVien>()
                .HasIndex(s => s.MaSV)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");

            // MaDN unique chỉ khi DeletedAt IS NULL
            builder.Entity<DoanhNghiep>()
                .HasIndex(d => d.MaDN)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");

            // MaNganh unique chỉ khi DeletedAt IS NULL
            builder.Entity<Nganh>()
                .HasIndex(d => d.MaNganh)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");

            // ===== Cấu hình mối quan hệ 1-1 =====

            // AppUser ↔ SinhVien
            builder.Entity<SinhVien>()
                .HasOne(sv => sv.NguoiDung)
                .WithOne()
                .HasForeignKey<SinhVien>(sv => sv.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);

            // AppUser ↔ DoanhNghiep
            builder.Entity<DoanhNghiep>()
                .HasOne(dn => dn.NguoiDung)
                .WithOne()
                .HasForeignKey<DoanhNghiep>(dn => dn.NguoiDungId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Cấu hình mối quan hệ 1-nhiều (để tránh multiple cascade paths) =====

            // DoanhNghiep ↔ ViecLam - SetNull (xóa doanh nghiệp → set null, giữ lịch sử)
            builder.Entity<ViecLam>()
                .HasOne(v => v.DoanhNghiep)
                .WithMany(dn => dn.ViecLams)
                .HasForeignKey(v => v.DoanhNghiepId)
                .OnDelete(DeleteBehavior.SetNull);

            // ViecLam ↔ LinhVuc - RESTRICT
            builder.Entity<ViecLam>()
                .HasOne(v => v.LinhVuc)
                .WithMany(l => l.ViecLams)
                .HasForeignKey(v => v.LinhVucId)
                .OnDelete(DeleteBehavior.Restrict);

            // ViecLam ↔ SinhVienViecLam - SetNull (xóa việc làm → set null, giữ lịch sử)
            builder.Entity<SinhVienViecLam>()
                .HasOne(sv => sv.ViecLam)
                .WithMany(v => v.SinhVienViecLams)
                .HasForeignKey(sv => sv.ViecLamId)
                .OnDelete(DeleteBehavior.SetNull);

            // SinhVien ↔ SinhVienViecLam - SetNull (xóa sinh viên → set null, giữ lịch sử)
            builder.Entity<SinhVienViecLam>()
                .HasOne(sv => sv.SinhVien)
                .WithMany(s => s.SinhVienViecLams)
                .HasForeignKey(sv => sv.SinhVienId)
                .OnDelete(DeleteBehavior.SetNull);

            // ViecLam ↔ LichPhongVan - RESTRICT để giữ lịch sử
            builder.Entity<LichPhongVan>()
                .HasOne(l => l.ViecLam)
                .WithMany(v => v.LichPhongVans)
                .HasForeignKey(l => l.ViecLamId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Cấu hình mối quan hệ nhiều-nhiều =====

            // 2. LinhVuc ↔ Nganh (qua LinhVucNganh)
            builder.Entity<LinhVucNganh>()
                .HasKey(ln => new { ln.LinhVucId, ln.NganhId });

            builder.Entity<LinhVucNganh>()
                .HasOne(ln => ln.LinhVuc)
                .WithMany(l => l.LinhVucNganhs)
                .HasForeignKey(ln => ln.LinhVucId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LinhVucNganh>()
                .HasOne(ln => ln.Nganh)
                .WithMany(n => n.LinhVucNganhs)
                .HasForeignKey(ln => ln.NganhId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. SinhVien ↔ ViecLam (qua LuuTru - lưu việc làm yêu thích)
            builder.Entity<LuuTru>()
                .HasKey(l => new { l.SinhVienId, l.ViecLamId });

            builder.Entity<LuuTru>()
                .HasOne(l => l.SinhVien)
                .WithMany(s => s.LuuTrus)
                .HasForeignKey(l => l.SinhVienId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LuuTru>()
                .HasOne(l => l.ViecLam)
                .WithMany(v => v.LuuTrus)
                .HasForeignKey(l => l.ViecLamId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. LichPhongVan ↔ SinhVienViecLam (qua DangKyPhongVan)
            builder.Entity<DangKyPhongVan>()
                .HasKey(dk => new { dk.LichPhongVanId, dk.SinhVienViecLamId });

            builder.Entity<DangKyPhongVan>()
                .HasOne(dk => dk.LichPhongVan)
                .WithMany(l => l.DangKyPhongVans)
                .HasForeignKey(dk => dk.LichPhongVanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DangKyPhongVan>()
                .HasOne(dk => dk.SinhVienViecLam)
                .WithMany(sv => sv.DangKyPhongVans)
                .HasForeignKey(dk => dk.SinhVienViecLamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
