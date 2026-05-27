using HopTacDoanhNghiep.Enums.BaiViet;
using HopTacDoanhNghiep.Enums.DonVi;
using HopTacDoanhNghiep.Enums.HoSo;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.Enums.LienHe;
using HopTacDoanhNghiep.Enums.NguoiDung;
using HopTacDoanhNghiep.Enums.ViecLam;
using HopTacDoanhNghiep.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // ================= MIGRATION =================
            await context.Database.MigrateAsync();

            // ================= DON VI =================
            if (!await context.DonVis.AnyAsync())
            {
                context.DonVis.AddRange(
                    new DonVi
                    {
                        TenDV = "Ban Giám Hiệu",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = false,
                        CreatedAt = DateTime.Now,
                    },
                    new DonVi
                    {
                        TenDV = "Phòng Công Tác Sinh Viên",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = false,
                        CreatedAt = DateTime.Now,
                    },
                    new DonVi
                    {
                        TenDV = "Khoa Cơ Khí",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = true,
                        CreatedAt = DateTime.Now,
                    },
                    new DonVi
                    {
                        TenDV = "Khoa Điện - Điện Tử",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = true,
                        CreatedAt = DateTime.Now,
                    },
                    new DonVi
                    {
                         TenDV = "Khoa Kỹ Thuật Xây Dựng",
                         TrangThai = DonViStatus.HoatDong,
                         NhanDoiTac = true,
                         CreatedAt = DateTime.Now,
                    },
                    new DonVi
                    {
                        TenDV = "Khoa Công Nghệ Hóa Học - Môi Trường",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = true,
                        CreatedAt = DateTime.Now,
                    },
                    new DonVi
                     {
                         TenDV = "Khoa Sư Phạm Công Nghiệp",
                         TrangThai = DonViStatus.HoatDong,
                         NhanDoiTac = true,
                         CreatedAt = DateTime.Now,
                     },
                    new DonVi
                    {
                        TenDV = "Khoa Công Nghệ Số",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = true,
                        CreatedAt = DateTime.Now,
                    }
                );

                await context.SaveChangesAsync();
            }

            // ================= ROLES =================
            string[] roles = { "Admin", "Company", "Student", "Officer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ================= ADMIN USER =================
            var adminEmail = "admin@system.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    HoTen = "Administrator"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var companyUser = await EnsureUserAsync(userManager, "company1", "company1@system.com", "Company@123", "Công ty ABC", "Company");
            var companyUser2 = await EnsureUserAsync(userManager, "company2", "company2@system.com", "Company@123", "Công ty XYZ", "Company");
            var studentUser = await EnsureUserAsync(userManager, "student1", "student1@system.com", "Student@123", "Nguyễn Văn A", "Student");
            var studentUser2 = await EnsureUserAsync(userManager, "student2", "student2@system.com", "Student@123", "Trần Thị B", "Student");
            var staffUser = await EnsureUserAsync(userManager, "staff1", "staff1@system.com", "Staff@123", "Cán bộ hệ thống", "Officer");

            var now = DateTime.UtcNow;

            // ================= DANH MUC BAI VIET =================
            if (!await context.DanhMucBaiViets.AnyAsync())
            {
                context.DanhMucBaiViets.AddRange(
                    new DanhMucBaiViet
                    {
                        Ten = "Tin tức - Sự kiện",
                        MoTa = "Tin tức, hoạt động và sự kiện nổi bật",
                        Slug = "tin-tuc-su-kien",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new DanhMucBaiViet
                    {
                        Ten = "Hướng nghiệp - Việc làm",
                        MoTa = "Chia sẻ kỹ năng và cơ hội việc làm cho sinh viên",
                        Slug = "huong-nghiep-viec-lam",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            // ================= CHUC VU + DON VI =================
            if (!await context.ChucVus.AnyAsync())
            {
                context.ChucVus.AddRange(
                    new ChucVu
                    {
                        TenChucVu = "Giám đốc",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new ChucVu
                    {
                        TenChucVu = "Chuyên viên",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });
            }

            if (!await context.DonVis.AnyAsync())
            {
                context.DonVis.AddRange(
                    new DonVi
                    {
                        TenDV = "Trung tâm Hợp tác Doanh nghiệp",
                        Tel = "028-12345678",
                        Email = "hoptac@system.com",
                        Website = "https://example.com",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = true,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new DonVi
                    {
                        TenDV = "Phòng Công tác Sinh viên",
                        Tel = "028-87654321",
                        Email = "ctsv@system.com",
                        Website = "https://example.com",
                        TrangThai = DonViStatus.HoatDong,
                        NhanDoiTac = true,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });
            }

            if (!await context.ChucVus.AnyAsync() || !await context.DonVis.AnyAsync())
            {
                await context.SaveChangesAsync();
            }

            var chucVuGiamDoc = await context.ChucVus.FirstAsync();
            var chucVuChuyenVien = await context.ChucVus.Skip(1).FirstAsync();
            var donViHopTac = await context.DonVis.FirstAsync();
            var donViCongTacSinhVien = await context.DonVis.Skip(1).FirstAsync();

            // ================= DOANH NGHIEP =================
            if (!await context.DoanhNghieps.AnyAsync())
            {
                context.DoanhNghieps.AddRange(
                    new DoanhNghiep
                    {
                        MaDN = "DN001",
                        TenHienThi = "Công ty ABC",
                        Website = "https://abc.example.com",
                        MaSoThue = "0101234567",
                        TenPhapLy = "Công ty Cổ phần ABC",
                        Hotline = "0901234567",
                        EmailCongTy = "contact@abc.example.com",
                        Logo = "/uploads/mock/company-abc.png",
                        DiaChi = "Quận 1, TP. Hồ Chí Minh",
                        GioiThieu = "Doanh nghiệp công nghệ tập trung vào giải pháp số.",
                        QuyMoNhanSu = 120,
                        MaNguoiDung = companyUser.Id,
                        NoiDungHopTac = "Hợp tác tuyển dụng và tiếp nhận sinh viên thực tập.",
                        TrangThaiHopTac = HopTacDoanhNghiepStatus.DuyetHopTac,
                        GhiChu = "Dữ liệu mẫu",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new DoanhNghiep
                    {
                        MaDN = "DN002",
                        TenHienThi = "Công ty XYZ",
                        Website = "https://xyz.example.com",
                        MaSoThue = "0107654321",
                        TenPhapLy = "Công ty TNHH XYZ",
                        Hotline = "0907654321",
                        EmailCongTy = "hr@xyz.example.com",
                        Logo = "/uploads/mock/company-xyz.png",
                        DiaChi = "Thành phố Thủ Đức, TP. Hồ Chí Minh",
                        GioiThieu = "Doanh nghiệp thương mại dịch vụ với nhu cầu tuyển dụng đa dạng.",
                        QuyMoNhanSu = 80,
                        MaNguoiDung = companyUser2.Id,
                        NoiDungHopTac = "Đang chờ xác nhận hợp tác.",
                        TrangThaiHopTac = HopTacDoanhNghiepStatus.ChoXuLy,
                        GhiChu = "Dữ liệu mẫu",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            var doanhNghiepAbc = await context.DoanhNghieps.FirstAsync();
            var doanhNghiepXyz = await context.DoanhNghieps.Skip(1).FirstAsync();

            // ================= CAN BO =================
            if (!await context.CanBos.AnyAsync())
            {
                context.CanBos.AddRange(
                    new CanBo
                    {
                        MaCB = "CB001",
                        MaChucVu = chucVuGiamDoc.MaChucVu,
                        MaDonVi = donViHopTac.MaDV,
                        MaNguoiDung = staffUser.Id,
                        BHTT = "BH001",
                        BHTN = "BHTN001",
                        STK = "0123456789",
                        AnhThe = "/uploads/mock/canbo-1.jpg",
                        GhiChu = "Cán bộ phụ trách hợp tác doanh nghiệp",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new CanBo
                    {
                        MaCB = "CB002",
                        MaChucVu = chucVuChuyenVien.MaChucVu,
                        MaDonVi = donViCongTacSinhVien.MaDV,
                        MaNguoiDung = adminUser.Id,
                        BHTT = "BH002",
                        BHTN = "BHTN002",
                        STK = "9876543210",
                        AnhThe = "/uploads/mock/canbo-2.jpg",
                        GhiChu = "Cán bộ hỗ trợ sinh viên",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            // ================= SINH VIEN =================
            if (!await context.SinhViens.AnyAsync())
            {
                context.SinhViens.AddRange(
                    new SinhVien
                    {
                        MaSV = "SV001",
                        EmailGiaoDuc = "sv001@edu.example.com",
                        MaNguoiDung = studentUser.Id,
                        HoSoNangLuc = "Kỹ năng lập trình C#, ASP.NET Core, SQL Server",
                        AnhThe = "/uploads/mock/student-1.jpg",
                        GhiChu = "Sinh viên năm cuối chuyên ngành CNTT",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new SinhVien
                    {
                        MaSV = "SV002",
                        EmailGiaoDuc = "sv002@edu.example.com",
                        MaNguoiDung = studentUser2.Id,
                        HoSoNangLuc = "Kỹ năng thiết kế UI, truyền thông và làm việc nhóm",
                        AnhThe = "/uploads/mock/student-2.jpg",
                        GhiChu = "Sinh viên mới tốt nghiệp",
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            var sinhVien1 = await context.SinhViens.FirstAsync();
            var sinhVien2 = await context.SinhViens.Skip(1).FirstAsync();

            // ================= TIN TUYEN DUNG =================
            if (!await context.TinTuyenDungs.AnyAsync())
            {
                context.TinTuyenDungs.AddRange(
                    new TinTuyenDung
                    {
                        TieuDe = "Thực tập sinh lập trình .NET",
                        Slug = "thuc-tap-sinh-lap-trinh-net",
                        MoTa = "Tham gia phát triển các tính năng web nội bộ.",
                        YeuCau = "Biết C#, HTML, CSS, SQL cơ bản.",
                        UuTien = "Ưu tiên sinh viên năm cuối.",
                        QuyenLoi = "Hỗ trợ dấu mộc, phụ cấp thực tập, cơ hội lên nhân viên chính thức.",
                        LuongToiThieu = 3000000,
                        LuongToiDa = 6000000,
                        DiaDiem = "TP. Hồ Chí Minh",
                        TuKhoa = "dotnet, csharp, internship",
                        NgayBatDau = now.AddDays(-7),
                        NgayHetHan = now.AddMonths(1),
                        LoaiViecLam = ViecLamType.ThucTap,
                        DoiTuongUngTuyen = DoiTuongUngTuyen.SinhVienNamCuoi,
                        TrinhDo = TrinhDoType.DaiHoc,
                        Status = ViecLamStatus.CongBo,
                        MaDoanhNgiep = doanhNghiepAbc.MaDN,
                        CreatedAt = now,
                        CreatedBy = companyUser.Id
                    },
                    new TinTuyenDung
                    {
                        TieuDe = "Nhân viên marketing nội dung",
                        Slug = "nhan-vien-marketing-noi-dung",
                        MoTa = "Xây dựng nội dung và quản lý kênh truyền thông.",
                        YeuCau = "Có kỹ năng viết bài, sử dụng công cụ văn phòng.",
                        UuTien = "Ưu tiên ứng viên có kinh nghiệm 1 năm.",
                        QuyenLoi = "Lương cạnh tranh, thưởng KPI, môi trường năng động.",
                        LuongToiThieu = 10000000,
                        LuongToiDa = 15000000,
                        DiaDiem = "Thành phố Thủ Đức",
                        TuKhoa = "marketing, content, fulltime",
                        NgayBatDau = now.AddDays(-10),
                        NgayHetHan = now.AddMonths(2),
                        LoaiViecLam = ViecLamType.ToanThoiGian,
                        DoiTuongUngTuyen = DoiTuongUngTuyen.DaTotNghiep,
                        TrinhDo = TrinhDoType.DaiHoc,
                        Status = ViecLamStatus.CongBo,
                        MaDoanhNgiep = doanhNghiepXyz.MaDN,
                        CreatedAt = now,
                        CreatedBy = companyUser2.Id
                    });

                await context.SaveChangesAsync();
            }

            var tinTuyenDung1 = await context.TinTuyenDungs.FirstAsync();
            var tinTuyenDung2 = await context.TinTuyenDungs.Skip(1).FirstAsync();

            // ================= BAI VIET =================
            if (!await context.BaiViets.AnyAsync())
            {
                var danhMucTinTuc = await context.DanhMucBaiViets.FirstAsync();
                var danhMucHuongNghiep = await context.DanhMucBaiViets.Skip(1).FirstAsync();

                context.BaiViets.AddRange(
                    new BaiViet
                    {
                        TieuDe = "Khai giảng chương trình kết nối doanh nghiệp 2026",
                        AnhMinhHoa = "/uploads/mock/bai-viet-1.jpg",
                        TacGia = "Administrator",
                        NoiDung = "Chương trình kết nối doanh nghiệp tạo thêm cơ hội thực tập và tuyển dụng cho sinh viên.",
                        Slug = "khai-giang-chuong-trinh-ket-noi-doanh-nghiep-2026",
                        TuKhoa = "doanh nghiep, sinh vien, hop tac",
                        TrangThai = BaiVietStatus.XuatBan,
                        MaDanhMuc = danhMucTinTuc.MaDanhMuc,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new BaiViet
                    {
                        TieuDe = "5 kỹ năng sinh viên cần chuẩn bị trước khi đi thực tập",
                        AnhMinhHoa = "/uploads/mock/bai-viet-2.jpg",
                        TacGia = "Administrator",
                        NoiDung = "Bài viết tổng hợp những kỹ năng mềm và kỹ năng chuyên môn cần thiết cho kỳ thực tập.",
                        Slug = "5-ky-nang-sinh-vien-can-chuan-bi-truoc-khi-di-thuc-tap",
                        TuKhoa = "thuc tap, sinh vien, ky nang",
                        TrangThai = BaiVietStatus.XuatBan,
                        MaDanhMuc = danhMucHuongNghiep.MaDanhMuc,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            // ================= HOP TAC DON VI =================
            if (!await context.HopTacDonVis.AnyAsync())
            {
                context.HopTacDonVis.AddRange(
                    new HopTacDonVi
                    {
                        MaDN = doanhNghiepAbc.MaDN,
                        MaDV = donViHopTac.MaDV,
                        TrangThai = HopTacDonViStatus.HopTac,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new HopTacDonVi
                    {
                        MaDN = doanhNghiepXyz.MaDN,
                        MaDV = donViCongTacSinhVien.MaDV,
                        TrangThai = HopTacDonViStatus.ChoPhanHoi,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            // ================= DON UNG TUYEN =================
            if (!await context.DonUngTuyens.AnyAsync())
            {
                context.DonUngTuyens.AddRange(
                    new DonUngTuyen
                    {
                        MaSV = sinhVien1.MaSV,
                        MaTTD = tinTuyenDung1.MaTTD,
                        HoSoUngTuyen = "/uploads/mock/ho-so-ung-tuyen-sv001.pdf",
                        TrangThai = HoSoStatus.ChoPhanHoi,
                        CreatedAt = now,
                        CreatedBy = studentUser.Id
                    },
                    new DonUngTuyen
                    {
                        MaSV = sinhVien2.MaSV,
                        MaTTD = tinTuyenDung2.MaTTD,
                        HoSoUngTuyen = "/uploads/mock/ho-so-ung-tuyen-sv002.pdf",
                        TrangThai = HoSoStatus.ChapNhan,
                        CreatedAt = now,
                        CreatedBy = studentUser2.Id
                    });

                await context.SaveChangesAsync();
            }

            // ================= LIEN HE =================
            if (!await context.LienHes.AnyAsync())
            {
                context.LienHes.AddRange(
                    new LienHe
                    {
                        HoTen = "Phạm Minh C",
                        Email = "minhc@example.com",
                        DienThoai = "0912345678",
                        NoiDung = "Tôi muốn tìm hiểu thêm về chương trình thực tập.",
                        TrangThai = LienHeStatus.ChoXuLy,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new LienHe
                    {
                        HoTen = "Lê Thu D",
                        Email = "thud@example.com",
                        DienThoai = "0987654321",
                        NoiDung = "Doanh nghiệp tôi muốn ký kết hợp tác với nhà trường.",
                        TrangThai = LienHeStatus.DaXuLy,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }

            // ================= THONG BAO =================
            if (!await context.ThongBaos.AnyAsync())
            {
                context.ThongBaos.AddRange(
                    new ThongBao
                    {
                        TieuDe = "Hệ thống đã sẵn sàng",
                        NoiDung = "Dữ liệu mẫu đã được khởi tạo thành công.",
                        MaNguoiDung = adminUser.Id,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    },
                    new ThongBao
                    {
                        TieuDe = "Có tin tuyển dụng mới",
                        NoiDung = "Bạn có thể xem các vị trí thực tập và việc làm phù hợp.",
                        MaNguoiDung = studentUser.Id,
                        CreatedAt = now,
                        CreatedBy = adminUser.Id
                    });

                await context.SaveChangesAsync();
            }
        }

        private static async Task<AppUser> EnsureUserAsync(
            UserManager<AppUser> userManager,
            string userName,
            string email,
            string password,
            string fullName,
            string? roleName)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    HoTen = fullName,
                    TrangThai = NguoiDungStatus.HoatDong
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join("; ", result.Errors.Select(x => x.Description));
                    throw new InvalidOperationException($"Không thể tạo user mẫu {email}: {errorMessage}");
                }
            }

            if (!string.IsNullOrWhiteSpace(roleName) && !await userManager.IsInRoleAsync(user, roleName))
            {
                await userManager.AddToRoleAsync(user, roleName);
            }

            return user;
        }
    }
}
