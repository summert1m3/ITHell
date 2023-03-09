using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;

namespace ITHell.VacancyParser.Domain.Services.Vacancy.VacancyCardParser;

public interface IVacancyCardParser
{
    public List<VacancyCard> ParseVacancyCardsFromDom(IDocument doc);
}