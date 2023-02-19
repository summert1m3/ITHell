using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage;

namespace ITHell.VacancyParser.Application.Services.Parsers;

public interface IHtmlParser
{
    public List<VacancyCard> ParseVacancyCards(IDocument doc);
    public VacancyPage ParseVacancyPage(
        IDocument doc, VacancyCard vacancyCard);
}