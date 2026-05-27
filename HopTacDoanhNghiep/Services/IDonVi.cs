using HopTacDoanhNghiep.ViewModels.Common;
using HopTacDoanhNghiep.ViewModels.DonVi;

namespace HopTacDoanhNghiep.Services
{
    public interface IDonVi
    {
        public Task<BaseResult<IEnumerable<DonViVM>>> GetDonViNhanHopTacs();
    }
}
