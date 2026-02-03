using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HopTacDoanhNghiep.Helpers
{
    public static class SlugHelper
    {
        public static string Generate(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // 1. Chuẩn hóa unicode (tách dấu)
            var normalized = input.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            var noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

            // 2. Đưa về lowercase
            var lower = noDiacritics.ToLowerInvariant();

            // 3. Thay ký tự không hợp lệ bằng dấu -
            var slug = Regex.Replace(lower, @"[^a-z0-9\s-]", "");

            // 4. Gom nhiều space / dash thành 1 dash
            slug = Regex.Replace(slug, @"[\s-]+", "-");

            // 5. Trim dash đầu và cuối
            slug = slug.Trim('-');

            return slug;
        }
    }
}
