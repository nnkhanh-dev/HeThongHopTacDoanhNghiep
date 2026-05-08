using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.Services;
using HopTacDoanhNghiep.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HopTacDoanhNghiep.Controllers
{
    public class LinhVucController : Controller
    {
        private readonly ILinhVuc _linhVuc;

        public LinhVucController(ILinhVuc linhVuc)
        {
            _linhVuc = linhVuc;
        }

        [HttpGet("danh-sach-linh-vuc")]
        public async Task<IActionResult> GetListLinhVuc(int pageIndex = 1, int pageSize = 10, string? keyword = null)
        {
            var result = await _linhVuc.GetListLinhVuc(pageIndex, pageSize, keyword);
            return Ok(result);
        }
    }
}
