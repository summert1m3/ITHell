using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage;

namespace ITHell.VacancyParser.Domain.Services.Vacancy.VacancyPageParser;

public interface IVacancyPageParser
{
    public VacancyPage ParseVacancyPageFromDom(IDocument doc, VacancyCard vacancyCard);
}