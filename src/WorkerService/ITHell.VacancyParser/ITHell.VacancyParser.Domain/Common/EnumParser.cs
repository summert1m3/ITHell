using System.ComponentModel;
using System.Reflection;

namespace ITHell.VacancyParser.Domain.Common;

public static class EnumParser
{
    public static T ParseEnum<T>(string description) where T : Enum
    {
        foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
        {
            var field = typeof(T).GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && attribute.Description == description)
            {
                return value;
            }
        }

        throw new ArgumentException($"Invalid value '{description}' for enum type {typeof(T).Name}");
    }
}