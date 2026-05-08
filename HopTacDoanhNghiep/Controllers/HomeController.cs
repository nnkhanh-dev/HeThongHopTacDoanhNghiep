using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Models;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Home;
using HopTacDoanhNghiep.ViewModels.LienHe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HopTacDoanhNghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaiViet _baiViet;
        private readonly IViecLam _viecLam;
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public HomeController(IBaiViet baiViet, IViecLam viecLam, AppDbContext db, IConfiguration config)
        {
            _baiViet = baiViet;
            _viecLam = viecLam;
            _db = db;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var tinResult = await _baiViet.GetListBaiViet(1, 12, null, "tin-tuc");
            var thongBaoResult = await _baiViet.GetListBaiViet(1, 12, null, "thong-bao");
            var viecResult = await _viecLam.GetListViecLam(1, 6, null, null, null, null, null, null, null, null, null);
            var baiVietResult = await _baiViet.GetListBaiViet(1, 12, null, "bai-viet");

            var model = new HomeVM
            {
                TinTucs = tinResult?.Records?.ToList(),
                ThongBaos = thongBaoResult?.Records?.ToList(),
                ViecLams = viecResult?.Records?.ToList(),
                BaiViets = baiVietResult?.Records?.ToList()
            };

            return View(model);
        }

        [HttpGet("/lien-he")]
        public IActionResult LienHe()
        {
            return View();
        }

        [HttpPost("/lien-he")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LienHe(LienHeVM model)
        {
            // Read recaptcha response from form
            var recaptchaResponse = Request.Form["g-recaptcha-response"].ToString();
            model.CapCha = recaptchaResponse;

            // Re-validate model state for CapCha after setting value
            if (ModelState.ContainsKey("CapCha"))
            {
                ModelState.Remove("CapCha");
            }

            if (!TryValidateModel(model))
            {
                return View(model);
            }

            // Verify reCAPTCHA with Google
            var secret = _config["GoogleReCaptcha:SecretKey"];
            using var http = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "secret", secret },
                { "response", recaptchaResponse }
            };

            var content = new FormUrlEncodedContent(values);
            var resp = await http.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var respStr = await resp.Content.ReadAsStringAsync();
            try
            {
                using var doc = JsonDocument.Parse(respStr);
                var root = doc.RootElement;
                var success = root.GetProperty("success").GetBoolean();
                if (!success)
                {
                    ModelState.AddModelError("CapCha", "Xác thực reCAPTCHA thất bại. Vui lòng thử lại.");
                    return View(model);
                }
            }
            catch
            {
                ModelState.AddModelError("CapCha", "Không thể xác thực reCAPTCHA. Vui lòng thử lại sau.");
                return View(model);
            }

            // Save contact to DB
            var lienHe = new LienHe
            {
                HoTen = model.HoTen,
                Email = model.Email,
                DienThoai = model.SDT,
                NoiDung = model.NoiDung,
                CreatedAt = DateTime.Now
            };

            _db.LienHes.Add(lienHe);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Gửi liên hệ thành công. Chúng tôi sẽ phản hồi sớm!";
            return RedirectToAction("LienHe");
        }
    }
}
