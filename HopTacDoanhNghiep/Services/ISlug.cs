using System.Linq.Expressions;

namespace HopTacDoanhNghiep.Services
{
    public interface ISlug
    {
        Task<string> GenerateUniqueSlugAsync<TEntity>(
            string input,
            IQueryable<TEntity> queryable,
            Expression<Func<TEntity, string>> slugSelector) where TEntity : class;
    }
}
