using System.ComponentModel;

namespace ITHell.VacancyParser.Domain.Common;

[AttributeUsage(AttributeTargets.All)]
public class MultipleDescriptionAttribute : Attribute
{
    public MultipleDescriptionAttribute(params string[] values)
    {
        this.Values = values;
    }

    public string[] Values { get; private set; }
}