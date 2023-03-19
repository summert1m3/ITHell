using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Domain.Common;
using ITHell.VacancyParser.Domain.Entities.Resume;

namespace ITHell.VacancyParser.Domain.Services.Resume.CardParser;

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
            var resumePageLink = resumeEl.QuerySelector("a.serp-item__title")?.GetAttribute("href");
            Guard.Against.NullOrWhiteSpace(resumePageLink);
            
            const string pattern = @"/resume/(.*?)\?query=";
            var match = Regex.Match(resumePageLink, pattern);

            Guid? resumeId = null;
            
            if (match.Success)
            {
                var result = match.Groups[1].Value;
    
                var validGuidStr = result.Substring(0, result.Length - 6);

                resumeId = Guid.Parse(validGuidStr);
            }
            Guard.Against.Null(resumeId);

            EmployeeStatus? employeeStatus = null;
            
            var label = 
                resumeEl.QuerySelector("div.resume-search-item__label div")?
                    .TextContent
                    .Replace('\u00A0', ' ');

            if (label is not null)
            {
                employeeStatus = EnumParser.ParseEnumDescription<EmployeeStatus>(label);
            }
            
            resumeCards.Add(
                new ResumeCard()
                {
                    ResumeId = (Guid)resumeId,
                    ResumePageLink = _jobSiteLink + resumePageLink,
                    EmployeeStatus = employeeStatus
                });
        }

        return resumeCards;
    }
}