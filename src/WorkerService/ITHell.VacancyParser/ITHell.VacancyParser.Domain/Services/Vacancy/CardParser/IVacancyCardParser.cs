using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Card;

namespace ITHell.VacancyParser.Domain.Services.Vacancy.CardParser;

public interface IVacancyCardParser
{
    public List<VacancyCard> ParseVacancyCardsFromDom(IDocument doc);
}