using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;

namespace ITHell.VacancyParser.Domain.Services.Resume.CardParser;

public interface IResumeCardParser
{
    public List<ResumeCard> ParseResumeCardsFromDom(IDocument doc);
}