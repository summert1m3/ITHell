using ITHell.VacancyParser.Domain.Common.Attributes;

namespace ITHell.VacancyParser.Domain.Common.Language;

/// <summary>
/// Уровни владения языком
/// </summary>
public enum LanguageLevel
{
    [MultipleDescription("A1")]
    A1,
    [MultipleDescription("A2")]
    A2,
    [MultipleDescription("B1")]
    B1,
    [MultipleDescription("B2")]
    B2,
    [MultipleDescription("C1")]
    C1,
    [MultipleDescription("C2")]
    C2,
    [MultipleDescription("Native", "Родной")]
    Native
}