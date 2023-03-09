using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage;
using ITHell.VacancyParser.Domain.Services.Resume.ResumeCardParser;
using ITHell.VacancyParser.Domain.Services.Resume.ResumePageParser;
using ITHell.VacancyParser.Domain.Services.Vacancy.VacancyCardParser;
using ITHell.VacancyParser.Domain.Services.Vacancy.VacancyPageParser;

namespace ITHell.VacancyParser.Application.Services.Parsers;

public class DefaultHtmlParser : IHtmlParser
{
    private readonly IVacancyCardParser _vacancyCardParser;
    private readonly IVacancyPageParser _vacancyPageParser;
    
    private readonly IResumeCardParser _resumeCardParser;
    private readonly IResumePageParser _resumePageParser;
    public DefaultHtmlParser(
        IVacancyCardParser vacancyCardParser, 
        IVacancyPageParser vacancyPageParser, 
        IResumeCardParser resumeCardParser, 
        IResumePageParser resumePageParser)
    {
        _vacancyCardParser = vacancyCardParser;
        _vacancyPageParser = vacancyPageParser;
        _resumeCardParser = resumeCardParser;
        _resumePageParser = resumePageParser;
    }
    public List<VacancyCard> ParseVacancyCards(IDocument doc)
    {
        return _vacancyCardParser.ParseVacancyCardsFromDom(doc);
    }

    public VacancyPage ParseVacancyPage(IDocument doc, VacancyCard vacancyCard)
    {
        return _vacancyPageParser.ParseVacancyPageFromDom(doc, vacancyCard);
    }

    public List<ResumeCard> ParseResumeCards(IDocument doc)
    {
        throw new NotImplementedException();
    }

    public ResumePage ParseResumePage(IDocument doc)
    {
        throw new NotImplementedException();
    }
}