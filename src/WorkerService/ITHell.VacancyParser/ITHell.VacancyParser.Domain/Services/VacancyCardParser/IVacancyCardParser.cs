using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;

namespace ITHell.VacancyParser.Domain.Services.VacancyCardParser;

public interface IVacancyCardParser
{
    public List<VacancyCard> ParseVacancyCardsFromHtmlDoc(IDocument doc);
}