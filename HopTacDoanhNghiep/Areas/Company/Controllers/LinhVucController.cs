using HopTacDoanhNghiep.Areas.Company.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = "Company")]
    public class LinhVucController : Controller
    {
        private readonly ILinhVucCompany _linhVuc;

        public LinhVucController(ILinhVucCompany linhVuc)
        {
            _linhVuc = linhVuc;
        }

        [HttpGet("doanh-nghiep/linh-vuc/danh-sach")]
        public async Task<IActionResult> GetListLinhVuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _linhVuc.GetListLinhVuc(pageIndex, pageSize, keyword);
            return Ok(result);
        }


    }
}
