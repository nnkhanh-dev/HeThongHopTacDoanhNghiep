
using HopTacDoanhNghiep.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HopTacDoanhNghiep.Services
{
    public class SlugService : ISlug
    {
        public async Task<string> GenerateUniqueSlugAsync<TEntity>(
            string input,
            IQueryable<TEntity> queryable,
            Expression<Func<TEntity, string>> slugSelector) where TEntity : class
        {
            var baseSlug = SlugHelper.Generate(input);

            if (string.IsNullOrWhiteSpace(baseSlug))
                return string.Empty;

            // Build expression: x => slugSelector(x) == baseSlug
            var parameter = slugSelector.Parameters[0];
            var equalExpression = Expression.Equal(
                slugSelector.Body,
                Expression.Constant(baseSlug)
            );
            var baseExistsLambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);

            // Kiểm tra xem slug gốc đã tồn tại chưa
            var baseExists = await queryable.Where(baseExistsLambda).AnyAsync();

            if (!baseExists)
                return baseSlug;

            // Build expression: x => slugSelector(x).StartsWith(baseSlug + "-")
            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!;
            var startsWithExpression = Expression.Call(
                slugSelector.Body,
                startsWithMethod,
                Expression.Constant(baseSlug + "-")
            );
            var startsWithLambda = Expression.Lambda<Func<TEntity, bool>>(startsWithExpression, parameter);

            // Lấy tất cả slugs có pattern: baseSlug-{number}
            var existingSlugs = await queryable
                .Where(startsWithLambda)
                .Select(slugSelector)
                .ToListAsync();

            // Tìm số lớn nhất trong các suffix
            var maxNumber = 0;
            foreach (var slug in existingSlugs)
            {
                var suffix = slug.Substring(baseSlug.Length + 1);
                if (int.TryParse(suffix, out var number))
                {
                    maxNumber = Math.Max(maxNumber, number);
                }
            }

            return $"{baseSlug}-{maxNumber + 1}";
        }
    }
}
