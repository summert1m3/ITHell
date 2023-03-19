using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.Page;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Card;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Page;
using ITHell.VacancyParser.Domain.Services.Resume.CardParser;
using ITHell.VacancyParser.Domain.Services.Resume.PageParser;
using ITHell.VacancyParser.Domain.Services.Vacancy.CardParser;
using ITHell.VacancyParser.Domain.Services.Vacancy.PageParser;

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
        return _resumeCardParser.ParseResumeCardsFromDom(doc);
    }

    public ResumePage ParseResumePage(IDocument doc, ResumeCard resumeCard)
    {
        return _resumePageParser.ParseResumePageFromDom(doc, resumeCard);
    }
}