using System.ComponentModel;
using System.Reflection;

namespace Qute.Shared.Extensions;

public static class EnumExtension
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        if (field != null)
        {
            DescriptionAttribute? attribute = field.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null) return attribute.Description;
        }
        return value.ToString();
    }

    public static int GetValue(this Enum value)
    {
        return Convert.ToInt32(value);
    }
}
