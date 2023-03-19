using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Card;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Page;

namespace ITHell.VacancyParser.Domain.Services.Vacancy.PageParser;

public interface IVacancyPageParser
{
    public VacancyPage ParseVacancyPageFromDom(IDocument doc, VacancyCard vacancyCard);
}