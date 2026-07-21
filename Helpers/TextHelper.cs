using System.Globalization;

namespace HsqvLogistica.Helpers;

public static class TextHelper
{
    public static string? Upper(string? value)
        => value?.Trim().ToUpperInvariant();

    public static string? Lower(string? value)
        => value?.Trim().ToLowerInvariant();

    public static string? Title(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            value.ToLowerInvariant());
    }
}