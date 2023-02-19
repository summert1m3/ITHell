using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage;

namespace ITHell.VacancyParser.Domain.Services.VacancyPageParser;

public interface IVacancyPageParser
{
    public VacancyPage ParseVacancyPageFromHtmlDoc(IDocument doc, VacancyCard vacancyCard);
}