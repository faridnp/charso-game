using System.Globalization;
using System.Text;

namespace Charso.Core
{
    /// <summary>Canonical comparison rules for Persian puzzle input. Rendering/shaping is a separate presentation concern.</summary>
    public static class PersianText
    {
        public static string NormalizeForMatch(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;
            string source = value.Normalize(NormalizationForm.FormKC);
            var result = new StringBuilder(source.Length);
            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                if (c == '\u0640' || c == '\u200c' || c == '\u200f' || c == '\u200e' || c == '\ufeff') continue;
                if (c == '\u064a' || c == '\u0649') c = '\u06cc';
                else if (c == '\u0643') c = '\u06a9';
                else if (c >= '\u0660' && c <= '\u0669') c = (char)('0' + (c - '\u0660'));
                else if (c >= '\u06f0' && c <= '\u06f9') c = (char)('0' + (c - '\u06f0'));

                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category == UnicodeCategory.NonSpacingMark ||
                    category == UnicodeCategory.SpacingCombiningMark ||
                    category == UnicodeCategory.EnclosingMark) continue;

                if (char.IsWhiteSpace(c))
                {
                    if (result.Length > 0 && result[result.Length - 1] != ' ') result.Append(' ');
                    continue;
                }
                result.Append(c);
            }
            return result.ToString().Trim();
        }
    }
}