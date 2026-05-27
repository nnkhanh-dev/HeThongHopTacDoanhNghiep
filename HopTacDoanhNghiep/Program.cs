using Hangfire;
using Hangfire.SqlServer;
using HopTacDoanhNghiep.Areas.Admin.Services;
using HopTacDoanhNghiep.Areas.Company.Services;
using HopTacDoanhNghiep.Areas.Student.Services;
using HopTacDoanhNghiep.Areas.Officer.Services;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Middlewares;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ===== AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"));
});

// ===== Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ===== Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/dang-nhap";
    options.LogoutPath = "/dang-xuat";
    options.AccessDeniedPath = "/opps";

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});


// ===== Hangfire
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(
              builder.Configuration.GetConnectionString("Default"),
              new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.FromSeconds(15),
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              });
});

builder.Services.AddHangfireServer();

// ===== Services
builder.Services.AddScoped<ISlug, SlugService>();
builder.Services.AddScoped<IFileStorage, FileStorageService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBaiViet, BaiVietService>();
builder.Services.AddScoped<IBaiVietAdmin, BaiVietAdminService>();
builder.Services.AddScoped<IDanhMucBaiVietAdmin, DanhMucBaiVietAdminService>();
builder.Services.AddScoped<IDonUngTuyenStudent, DonUngTuyenStudentService>();
builder.Services.AddScoped<IDonUngTuyenCompany, DonUngTuyenCompanyService>();
builder.Services.AddScoped<ITinTuyenDungStudent, TinTuyenDungStudentService>();
builder.Services.AddScoped<ITinTuyenDungCompany, TinTuyenDungCompanyService>();
builder.Services.AddScoped<ITinTuyenDung, TinTuyenDungService>();
builder.Services.AddScoped<IDonVi, DonViService>();
builder.Services.AddScoped<IDoanhNghiepOfficer, DoanhNghiepOfficerService>();
builder.Services.AddScoped<IDonViAdmin, DonViAdminService>();
builder.Services.AddScoped<IChucVuAdmin, ChucVuAdminService>();
builder.Services.AddScoped<ICanBoAdmin, CanBoAdminService>();

var app = builder.Build();

// SEED DATABASE
await DbInitializer.SeedAsync(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire"); app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireAdminAuthorizationFilter()
    }
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
