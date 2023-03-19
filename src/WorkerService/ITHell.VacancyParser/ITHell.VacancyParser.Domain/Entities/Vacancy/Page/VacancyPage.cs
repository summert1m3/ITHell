using ITHell.VacancyParser.Domain.Entities.Vacancy.Page.ValueObjects;

namespace ITHell.VacancyParser.Domain.Entities.Vacancy.Page;

public class VacancyPage : BaseEntity
{
    public required int VacancyId { get; init; }
    public required string Title { get; init; }
    public required string PageLink { get; init; }
    public required Salary Salary { get; init; }
    public required Experience Experience { get; init; }
    public required List<string> WorkSchedules { get; init; }

    public required Company Company { get; init; }
    
    public string? VacancyLocation { get; init; }
    public required string MainContent { get; init; }
    
    /// <summary>
    /// Ключевые навыки
    /// </summary>
    public required List<string> TagList { get; init; }

    public required DateTime VacancyCreatedDate { get; init; }

    public override string ToString()
    {
        return $"{nameof(VacancyId)}: {VacancyId}, {nameof(Title)}: {Title}, " +
               $"{nameof(PageLink)}: {PageLink}, {nameof(Salary)}: {Salary}, " +
               $"{nameof(Experience)}: {Experience}, {nameof(WorkSchedules)}: {WorkSchedules}, " +
               $"{nameof(Company)}: {Company}, {nameof(VacancyLocation)}: {VacancyLocation}, " +
               $"{nameof(MainContent)}: {MainContent}, {nameof(TagList)}: {TagList}, " +
               $"{nameof(VacancyCreatedDate)}: {VacancyCreatedDate}";
    }
}