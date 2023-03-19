using System.ComponentModel;
using System.Reflection;
using ITHell.VacancyParser.Domain.Common.Attributes;

namespace ITHell.VacancyParser.Domain.Common;

public static class EnumParser
{
    public static T ParseEnumDescription<T>(string description) where T : Enum
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
    
    public static T ParseEnumMultipleDescription<T>(string description) where T : struct, Enum
    {
        foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
        {
            var field = typeof(T).GetField(value.ToString());
            var attribute = field.GetCustomAttribute<MultipleDescriptionAttribute>();
            
            if (attribute is not null)
            {
                if (attribute.Values.Contains(description))
                {
                    return value;
                }
            }
            else
            {
                if (Enum.TryParse<T>(description, out _))
                {
                    return value;
                }
            }
        }

        throw new ArgumentException($"Invalid value '{description}' for enum type {typeof(T).Name}");
    }
}