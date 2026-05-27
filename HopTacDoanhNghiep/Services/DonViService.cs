using HopTacDoanhNghiep.Data;
using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.DonVi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopTacDoanhNghiep.Services
{
    public class DonViService : IDonVi
    {
        private readonly AppDbContext _context;

        public DonViService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResult<IEnumerable<DonViVM>>> GetDonViNhanHopTacs()
        {
            var items = await _context.DonVis
                .AsNoTracking()
                .Where(x => x.DeletedAt == null && x.NhanDoiTac == true)
                .Select(x => new DonViVM
                {
                    MaDV = x.MaDV,
                    TenDV = x.TenDV
                })
                .ToListAsync();

            return BaseResult<IEnumerable<DonViVM>>.Success(items, "Lấy danh sách đơn vị thành công");
        }
    }
}
