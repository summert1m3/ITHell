namespace ITHell.VacancyParser.Domain.Common.Language;

/// <summary>
/// Уровни владения языком
/// </summary>
public enum LanguageLevel
{
    A1,
    A2,
    B1,
    B2,
    C1,
    C2,
    [MultipleDescription("Native", "Родной")]
    Native
}