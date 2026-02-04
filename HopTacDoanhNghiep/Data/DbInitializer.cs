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

            // ================= ROLES =================
            string[] roles = { "Admin", "Company", "Student" };

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

            // ================= COMPANY USER =================
            var companyEmail = "company@test.com";
            var companyUser = await userManager.FindByEmailAsync(companyEmail);

            if (companyUser == null)
            {
                companyUser = new AppUser
                {
                    UserName = "company",
                    Email = companyEmail,
                    EmailConfirmed = true,
                    HoTen = "Công ty Test",
                    PhoneNumber = "0901234567"
                };

                var result = await userManager.CreateAsync(companyUser, "Company@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(companyUser, "Company");

                    // Tạo thông tin DoanhNghiep
                    var doanhNghiep = new DoanhNghiep
                    {
                        Id = Guid.NewGuid(),
                        MaDN = "DN001",
                        TenHienThi = "Công ty TNHH Test",
                        TenPhapLy = "Công ty TNHH Thương mại Test",
                        Email = companyEmail,
                        SDT = "0901234567",
                        Website = "https://test.com",
                        MaSoThue = "0123456789",
                        NgayThanhLap = new DateTime(2020, 1, 1),
                        DiaChi = "123 Đường Test, Quận 1, TP.HCM",
                        GioiThieu = "Công ty chuyên về công nghệ thông tin",
                        QuyMoNhanSu = 50,
                        NguoiDungId = companyUser.Id
                    };

                    context.DoanhNghieps.Add(doanhNghiep);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(companyUser, "Company"))
                {
                    await userManager.AddToRoleAsync(companyUser, "Company");
                }
            }

            // ================= STUDENT USER =================
            var studentEmail = "student@test.com";
            var studentUser = await userManager.FindByEmailAsync(studentEmail);

            if (studentUser == null)
            {
                studentUser = new AppUser
                {
                    UserName = "student",
                    Email = studentEmail,
                    EmailConfirmed = true,
                    HoTen = "Nguyễn Văn A",
                    PhoneNumber = "0987654321"
                };

                var result = await userManager.CreateAsync(studentUser, "Student@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(studentUser, "Student");

                    // Tạo thông tin SinhVien
                    var sinhVien = new SinhVien
                    {
                        Id = Guid.NewGuid(),
                        MaSV = "SV001",
                        HoTen = "Nguyễn Văn A",
                        NgaySinh = new DateTime(2002, 5, 15),
                        Lop = "CNTT K15",
                        Khoa = "Công nghệ thông tin",
                        ChuyenNganh = "Kỹ thuật phần mềm",
                        Email = studentEmail,
                        SDT = "0987654321",
                        TimViec = true,
                        NguoiDungId = studentUser.Id
                    };

                    context.SinhViens.Add(sinhVien);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(studentUser, "Student"))
                {
                    await userManager.AddToRoleAsync(studentUser, "Student");
                }
            }
        }
    }
}
