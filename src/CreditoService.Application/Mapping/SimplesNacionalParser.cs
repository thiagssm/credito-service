using System.Globalization;
using System.Text;

namespace CreditoService.Application.Mapping;

public static class SimplesNacionalParser
{
    private static readonly HashSet<string> TrueValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "sim",
        "s",
        "true"
    };

    private static readonly HashSet<string> FalseValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "nao",
        "n",
        "false"
    };

    public static bool Parse(string value)
    {
        var normalized = Normalize(value);

        if (TrueValues.Contains(normalized))
        {
            return true;
        }

        if (FalseValues.Contains(normalized))
        {
            return false;
        }

        throw new ArgumentException("SimplesNacional deve ser informado como Sim ou Nao.", nameof(value));
    }

    public static bool IsValid(string value)
    {
        var normalized = Normalize(value);
        return TrueValues.Contains(normalized) || FalseValues.Contains(normalized);
    }

    public static string Format(bool value)
    {
        return value ? "Sim" : "Não";
    }

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var formD = value.Trim().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(formD.Length);

        foreach (var character in formD)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
    }
}
