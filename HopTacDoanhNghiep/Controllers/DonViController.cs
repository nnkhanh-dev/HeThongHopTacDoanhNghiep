using HopTacDoanhNghiep.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Controllers
{
    public class DonViController : Controller
    {
        private readonly IDonVi _donViService;

        public DonViController(IDonVi donViService)
        {
            _donViService = donViService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/don-vi-nhan-hop-tac")]
        public async Task<IActionResult> GetAll(string? q = null)
        {
            var result = await _donViService.GetDonViNhanHopTacs();
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.Message });

            var items = result.Data ?? Enumerable.Empty<HopTacDoanhNghiep.ViewModels.DonVi.DonViVM>();

            if (!string.IsNullOrWhiteSpace(q))
            {
                items = items.Where(x => x.TenDV.Contains(q.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            var select2Items = items
                .OrderBy(x => x.TenDV)
                .Select(x => new { id = x.MaDV, text = x.TenDV })
                .ToList();

            return Json(new { results = select2Items, pagination = new { more = false } });
        }
    }
}
