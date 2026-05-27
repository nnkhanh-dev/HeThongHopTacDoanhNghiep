using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Enums.HopTac;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.ViewModels.Account;
using HopTacDoanhNghiep.ViewModels.DonVi;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            if (roles.Contains("Officer"))
                return RedirectToAction("Index", "Home", new { area = "Officer" });
            // default to Student area if present
            if (roles.Contains("Student"))
                return RedirectToAction("Index", "Home", new { area = "Student" });

            return RedirectToAction("Index", "Home");
        }

        private async Task<IActionResult> ViewDangKyDoanhNghiepAsync(DangKyDoanhNghiepVM model)
        {
            var selectedIds = model.SelectedDonViIds
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            model.SelectedDonVis = selectedIds.Count == 0
                ? new List<DonViVM>()
                : await _context.DonVis
                    .AsNoTracking()
                    .Where(x => x.DeletedAt == null && x.NhanDoiTac && selectedIds.Contains(x.MaDV))
                    .OrderBy(x => x.TenDV)
                    .Select(x => new DonViVM
                    {
                        MaDV = x.MaDV,
                        TenDV = x.TenDV
                    })
                    .ToListAsync();

            return View(model);
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

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Company"))
            {
                var doanhNghiep = await _context.DoanhNghieps
                    .FirstOrDefaultAsync(dn => dn.MaNguoiDung == user.Id);

                if (doanhNghiep == null ||
                    doanhNghiep.TrangThaiHopTac != HopTacDoanhNghiepStatus.DuyetHopTac)
                {
                    ModelState.AddModelError(string.Empty,
                        "Thông tin hoặc trạng thái hợp tác chưa được phê duyệt");

                    return View(model);
                }
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

            var existingClaims = await _userManager.GetClaimsAsync(user);

            // Xóa claims cũ để cập nhật lại
            var fullNameClaim = existingClaims.FirstOrDefault(c => c.Type == "FullName");
            var avatarClaim = existingClaims.FirstOrDefault(c => c.Type == "Avatar");
            
            if (fullNameClaim != null)
                await _userManager.RemoveClaimAsync(user, fullNameClaim);
            if (avatarClaim != null)
                await _userManager.RemoveClaimAsync(user, avatarClaim);

           await _userManager.AddClaimAsync(user,
                    new Claim("FullName", user.HoTen ?? user.UserName ?? "Admin"));

            if (!string.IsNullOrEmpty(user.AnhDaiDien))
                await _userManager.AddClaimAsync(user, new Claim("Avatar", user.AnhDaiDien));

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

        [HttpGet("dang-ky-doanh-nghiep")]
        public async Task<IActionResult> DangKyDoanhNghiep()
        {
            return View(new DangKyDoanhNghiepVM());
        }

        [HttpPost("dang-ky-doanh-nghiep")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKyDoanhNghiep(DangKyDoanhNghiepVM model)
        {
            if (!ModelState.IsValid)
                return await ViewDangKyDoanhNghiepAsync(model);

            var existingEmail = await _userManager.FindByEmailAsync(model.EmailNguoiDaiDien);
            if (existingEmail != null)
            {
                ModelState.AddModelError(nameof(model.EmailNguoiDaiDien), "Email đã được sử dụng");
                return await ViewDangKyDoanhNghiepAsync(model);
            }

            var existingDN = await _context.DoanhNghieps.AnyAsync(x => x.MaSoThue == model.MaSoThue || x.EmailCongTy == model.EmailCongTy);

            if (existingDN)
            {
                ModelState.AddModelError(string.Empty, "Doanh nghiệp với mã số thuế hoặc email công ty đã tồn tại");
                return await ViewDangKyDoanhNghiepAsync(model);
            }

            var user = new AppUser
            {
                UserName = $"DN{model.MaSoThue.Trim()}",
                Email = model.EmailNguoiDaiDien,
                PhoneNumber = model.SoDienThoaiNguoiDaiDien,
                HoTen = model.HoTenNguoiDaiDien
            };

            var password = "Company@123";

            bool userCreated = false;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var createResult = await _userManager.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        foreach (var err in createResult.Errors)
                            ModelState.AddModelError(string.Empty, err.Description);
                        return await ViewDangKyDoanhNghiepAsync(model);
                    }

                    userCreated = true;

                    // Ensure Company role exists and assign
                    var roleName = "Company";
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!addRoleResult.Succeeded)
                    {
                        foreach (var err in addRoleResult.Errors)
                            ModelState.AddModelError(string.Empty, err.Description);
                        throw new Exception("Failed to assign role to user.");
                    }

                    // Create doanh nghiệp record in pending state
                    var doanhNghiep = new DoanhNghiep
                    {
                        MaDN = $"DN{model.MaSoThue.Trim()}",
                        TenHienThi = model.TenHienThiDoanhNghiep,
                        TenPhapLy = model.TenPhapLyDoanhNghiep,
                        MaSoThue = model.MaSoThue.Trim(),
                        Website = model.Website,
                        Hotline = model.Hotline,
                        EmailCongTy = model.EmailCongTy,
                        NoiDungHopTac = model.NoiDungHopTac,
                        MaNguoiDung = user.Id,
                        TrangThaiHopTac = HopTacDoanhNghiepStatus.ChoXuLy,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.DoanhNghieps.Add(doanhNghiep);
                    await _context.SaveChangesAsync();

                    var selectedDonViIds = model.SelectedDonViIds
                        .Where(id => id > 0)
                        .Distinct()
                        .ToList();

                    if (selectedDonViIds.Count > 0)
                    {
                        var validDonViIds = await _context.DonVis
                            .AsNoTracking()
                            .Where(x => x.DeletedAt == null && x.NhanDoiTac && selectedDonViIds.Contains(x.MaDV))
                            .Select(x => x.MaDV)
                            .ToListAsync();

                        var hopTacDonVis = validDonViIds
                            .Select(maDV => new HopTacDonVi
                            {
                                MaDN = doanhNghiep.MaDN,
                                MaDV = maDV,
                                TrangThai = HopTacDonViStatus.ChoPhanHoi,
                                CreatedAt = DateTime.UtcNow
                            })
                            .ToList();

                        _context.HopTacDonVis.AddRange(hopTacDonVis);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Đăng ký thành công. Chúng tôi sẽ liên hệ sau khi duyệt.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();

                    if (userCreated)
                    {
                        var deleteResult = await _userManager.DeleteAsync(user);
                    }

                    ModelState.AddModelError(string.Empty, "Lỗi khi đăng ký, vui lòng thử lại.");
                    return await ViewDangKyDoanhNghiepAsync(model);
                }
            }
        }

        [HttpPost("dang-xuat")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
