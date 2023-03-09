using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;

namespace ITHell.VacancyParser.Domain.Services.Resume.ResumeCardParser;

public class ResumeCardParser : IResumeCardParser
{
    private readonly string _jobSiteLink;

    public ResumeCardParser(string jobSiteLink)
    {
        _jobSiteLink = jobSiteLink;
    }

    public List<ResumeCard> ParseResumeCardsFromDom(IDocument doc)
    {
        var resumeCardEls = doc.QuerySelectorAll("div.serp-item");

        if (!resumeCardEls.Any())
        {
            throw new Exception("Не найдено ни одного резюме");
        }

        List<ResumeCard> resumeCards = new();
        foreach (var resumeEl in resumeCardEls)
        {
            
        }

        return resumeCards;
    }
}