using Qute.Rfb.Shared.Enums;

namespace Qute.Rfb.Api.Helpers;

public static class MigrationHelper
{
    public static int GetInteger(this string value)
    {
        value = value.Replace("\"", "");
        return int.Parse(value);
    }

    public static int? GetIntegerOrNull(this string value)
    {
        value = value.Replace("\"", "");
        if (!string.IsNullOrEmpty(value))
        {
            return int.Parse(value);
        }
        return null;
    }

    public static bool GetBoolean(this string value)
    {
        value = value.Replace("\"", "");
        return value == "S" || value == "s" ? true : false;
    }

    public static bool? GetBooleanOrNull(this string value)
    {
        value = value.Replace("\"", "");
        if (!string.IsNullOrEmpty(value))
        {
            return value == "S" || value == "s" ? true : false;
        }
        return null;
    }

    public static DateOnly GetDateOnly(this string value)
    {
        value = value.Replace("\"", "");
        return new DateOnly(
            int.Parse(value.Substring(0,4)), 
            int.Parse(value.Substring(4,2)), 
            int.Parse(value.Substring(6,2))
        );
    }

    public static DateOnly? GetDateOnlyOrNull(this string value)
    {
        value = value.Replace("\"", "");
        if (!string.IsNullOrEmpty(value) && value != "00000000")
        {
            return new DateOnly(
                int.Parse(value.Substring(0, 4)),
                int.Parse(value.Substring(4, 2)),
                int.Parse(value.Substring(6, 2))
            );
        }
        return null;
    }

    public static string GetString(this string value)
    {
        return value.Replace("\"", "");
    }

    public static string? GetStringOrNull(this string value)
    {
        value = value.Replace("\"", "");
        return string.IsNullOrEmpty(value) ? null : value;
    }

    public static decimal? GetDecimalOrNull(this string value)
    {
        value = value.Replace("\"", "");
        if (!string.IsNullOrEmpty(value))
        {
            var number = decimal.Parse(value);
            return number;
        }
        return null;
    }

    public static PorteEmpresa? GetPorteEmpresaValue(this string value)
    {
        value = value.Replace("\"", "");
        if (!string.IsNullOrEmpty(value))
        {
            var i = int.Parse(value);
            return (PorteEmpresa)i;
        }
        return null;
    }

}
