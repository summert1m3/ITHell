using ITHell.VacancyParser.Domain.Common.Language;

namespace ITHell.VacancyParser.Domain.Entities.Resume.Page.ValueObjects;

public class LanguageKnowledge
{
    public Language Language { get; init; }
    public LanguageLevel LanguageLevel { get; init; }
}