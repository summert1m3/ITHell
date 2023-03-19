namespace ITHell.VacancyParser.Domain.Common.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class MultipleDescriptionAttribute : Attribute
{
    public MultipleDescriptionAttribute(params string[] values)
    {
        this.Values = values;
    }

    public string[] Values { get; private set; }
}