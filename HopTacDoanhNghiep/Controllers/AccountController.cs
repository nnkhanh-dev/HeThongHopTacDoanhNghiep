using HopTacDoanhNghiep.ViewModels.Account;
using HopTacDoanhNghiep.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HopTacDoanhNghiep.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Thêm claims cho họ tên và avatar
                var existingClaims = await _userManager.GetClaimsAsync(user);
                
                if (!existingClaims.Any(c => c.Type == "FullName"))
                {
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", user.HoTen ?? user.UserName ?? "User"));
                }
                
                if (!existingClaims.Any(c => c.Type == "Avatar") && !string.IsNullOrEmpty(user.Avatar))
                {
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Avatar", user.Avatar));
                }
                
                // Refresh sign in để claims có hiệu lực
                await _signInManager.RefreshSignInAsync(user);
                
                return await RedirectToAreaForUser(user);
            }

            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        [HttpGet("dang-ky")]
        public async Task<IActionResult> Register()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var current = await _userManager.GetUserAsync(User);
                return await RedirectToAreaForUser(current);
            }

            return View();
        }

        [HttpPost("dang-ky")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Username), "Tài khoản đã tồn tại");
                return View(model);
            }

            var existingEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email đã được sử dụng");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                HoTen = model.FullName
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return View(model);
            }

            // Ensure Student role exists
            var roleName = "Student";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            // Thêm claims cho họ tên
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", user.HoTen ?? user.UserName ?? "User"));

            await _signInManager.SignInAsync(user, isPersistent: false);

            return await RedirectToAreaForUser(user);
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
