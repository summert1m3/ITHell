using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage;
using ITHell.VacancyParser.Domain.Services.VacancyCardParser;
using ITHell.VacancyParser.Domain.Services.VacancyPageParser;

namespace ITHell.VacancyParser.Application.Services.Parsers;

public class DefaultHtmlParser : IHtmlParser
{
    private readonly IVacancyCardParser _vacancyCardParser;
    private readonly IVacancyPageParser _vacancyPageParser;
    public DefaultHtmlParser(
        IVacancyCardParser vacancyCardParser, IVacancyPageParser vacancyPageParser)
    {
        _vacancyCardParser = vacancyCardParser;
        _vacancyPageParser = vacancyPageParser;
    }
    public List<VacancyCard> ParseVacancyCards(IDocument doc)
    {
        return _vacancyCardParser.ParseVacancyCardsFromHtmlDoc(doc);
    }

    public VacancyPage ParseVacancyPage(IDocument doc, VacancyCard vacancyCard)
    {
        return _vacancyPageParser.ParseVacancyPageFromHtmlDoc(doc, vacancyCard);
    }
}