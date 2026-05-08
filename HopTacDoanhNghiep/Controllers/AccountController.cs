using HopTacDoanhNghiep.Areas.Admin.ViewModels.DoanhNghiep;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HopTacDoanhNghiep.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        private async Task<IActionResult> RedirectToAreaForUser(AppUser user)
        {
            if (user == null) return RedirectToAction("Index", "Home");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            if (roles.Contains("Company"))
                return RedirectToAction("Index", "Home", new { area = "Company" });
            // default to Student area if present
            if (roles.Contains("Student"))
                return RedirectToAction("Index", "Home", new { area = "Student" });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("dang-nhap")]
        public async Task<IActionResult> Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var current = await _userManager.GetUserAsync(User);
                return await RedirectToAreaForUser(current);
            }

            return View();
        }

        [HttpPost("dang-nhap")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại");
                return View(model);
            }

            // Kiểm tra trạng thái tài khoản AppUser
            if (user.TrangThai != Enums.NguoiDung.NguoiDungStatus.HoatDong)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa liên hệ với quản trị viên để được hỗ trợ.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            // ===== Chỉ chạy khi login thành công =====
            // Đăng xuất để thêm claims và sign in lại
            await _signInManager.SignOutAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var existingClaims = await _userManager.GetClaimsAsync(user);

            // Xóa claims cũ để cập nhật lại
            var fullNameClaim = existingClaims.FirstOrDefault(c => c.Type == "FullName");
            var avatarClaim = existingClaims.FirstOrDefault(c => c.Type == "Avatar");
            
            if (fullNameClaim != null)
                await _userManager.RemoveClaimAsync(user, fullNameClaim);
            if (avatarClaim != null)
                await _userManager.RemoveClaimAsync(user, avatarClaim);

            // Thêm claims dựa theo role
            if (roles.Contains("Admin"))
            {
                // Admin: lấy HoTen và Avatar từ AppUser
                await _userManager.AddClaimAsync(user, 
                    new Claim("FullName", user.HoTen ?? user.UserName ?? "Admin"));
                
                if (!string.IsNullOrEmpty(user.Avatar))
                    await _userManager.AddClaimAsync(user, new Claim("Avatar", user.Avatar));
            }
            else if (roles.Contains("Company"))
            {
                // Company: lấy TenHienThi và Logo từ DoanhNghiep
                var doanhNghiep = await _context.DoanhNghieps
                    .FirstOrDefaultAsync(dn => dn.NguoiDungId == user.Id);
                
                if (doanhNghiep != null)
                {
                    await _userManager.AddClaimAsync(user, 
                        new Claim("FullName", doanhNghiep.TenHienThi ?? user.HoTen ?? "Company"));
                    
                    if (!string.IsNullOrEmpty(doanhNghiep.Logo))
                        await _userManager.AddClaimAsync(user, new Claim("Avatar", doanhNghiep.Logo));

                    await _userManager.AddClaimAsync(user, new Claim("IdNguoiDung", doanhNghiep.Id.ToString()));

                }
                else
                {
                    await _userManager.AddClaimAsync(user, 
                        new Claim("FullName", user.HoTen ?? user.UserName ?? "Company"));
                }
            }
            else if (roles.Contains("Student"))
            {
                // Student: lấy HoTen và AnhThe từ SinhVien
                var sinhVien = await _context.SinhViens
                    .FirstOrDefaultAsync(sv => sv.NguoiDungId == user.Id);
                
                if (sinhVien != null)
                {
                    await _userManager.AddClaimAsync(user, 
                        new Claim("FullName", sinhVien.HoTen ?? user.HoTen ?? "Student"));
                    
                    if (!string.IsNullOrEmpty(sinhVien.AnhThe))
                        await _userManager.AddClaimAsync(user, new Claim("Avatar", sinhVien.AnhThe));

                    await _userManager.AddClaimAsync(user, new Claim("IdNguoiDung", sinhVien.Id.ToString()));
                }
                else
                {
                    await _userManager.AddClaimAsync(user, 
                        new Claim("FullName", user.HoTen ?? user.UserName ?? "Student"));
                }
            }

            // Sign in lại với claims mới
            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

            return await RedirectToAreaForUser(user);
        }

        // [HttpGet("dang-ky")]
        // public async Task<IActionResult> Register()
        // {
        //     if (User?.Identity?.IsAuthenticated == true)
        //     {
        //         var current = await _userManager.GetUserAsync(User);
        //         return await RedirectToAreaForUser(current);
        //     }

        //     return View();
        // }

        // [HttpPost("dang-ky")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Register(RegisterVM model)
        // {
        //     if (!ModelState.IsValid)
        //         return View(model);

        //     var existingUser = await _userManager.FindByNameAsync(model.Username);
        //     if (existingUser != null)
        //     {
        //         ModelState.AddModelError(nameof(model.Username), "Tài khoản đã tồn tại");
        //         return View(model);
        //     }

        //     var existingEmail = await _userManager.FindByEmailAsync(model.Email);
        //     if (existingEmail != null)
        //     {
        //         ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng");
        //         return View(model);
        //     }

        //     var user = new AppUser
        //     {
        //         UserName = model.Username,
        //         Email = model.Email,
        //         PhoneNumber = model.PhoneNumber,
        //         HoTen = model.FullName
        //     };

        //     var createResult = await _userManager.CreateAsync(user, model.Password);
        //     if (!createResult.Succeeded)
        //     {
        //         foreach (var err in createResult.Errors)
        //             ModelState.AddModelError(string.Empty, err.Description);
        //         return View(model);
        //     }

        //     // Ensure Student role exists
        //     var roleName = "Student";
        //     if (!await _roleManager.RoleExistsAsync(roleName))
        //     {
        //         await _roleManager.CreateAsync(new IdentityRole(roleName));
        //     }

        //     await _userManager.AddToRoleAsync(user, roleName);

        //     // Thêm claims cho họ tên
        //     await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", user.HoTen ?? user.UserName ?? "User"));

        //     await _signInManager.SignInAsync(user, isPersistent: false);

        //     return await RedirectToAreaForUser(user);
        // }

        [HttpPost("dang-xuat")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
