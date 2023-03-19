using ITHell.VacancyParser.Domain.Common;
using ITHell.VacancyParser.Domain.Entities.Resume.Page.ValueObjects;

namespace ITHell.VacancyParser.Domain.Entities.Resume.Page;

public class ResumePage : BaseEntity
{
    /// <summary>
    /// Id резюме из ссылки
    /// </summary>
    public required Guid ResumeId { get; init; }
    
    /// <summary>
    /// Ссылка на страницу резюме
    /// </summary>
    public required string PageLink { get; init; }
    
    /// <summary>
    /// Статус поиска работы
    /// </summary>
    public required EmployeeStatus? EmployeeStatus { get; init; }

    public Gender? Gender { get; init; }
    public int? Age { get; init; }
    
    public DateTime? BirthDate { get; init; }
    
    /// <summary>
    /// Есть ли фото
    /// </summary>
    public bool HasPicture { get; init; }
    
    /// <summary>
    /// Предполагаемая зарплата
    /// </summary>
    public Salary? Salary { get; init; }
    public required string Title { get; init; }
    
    /// <summary>
    /// Занятость
    /// </summary>
    public required List<Employment> Employments { get; init; }
    
    /// <summary>
    /// График работы
    /// </summary>
    public required List<WorkSchedule> WorkSchedules { get; init; }
    
    public TimeSpan? Experience { get; init; }

    public string? AboutMe { get; init; }
    
    /// <summary>
    /// Ключевые навыки
    /// </summary>
    public required List<string> TagList { get; init; }
    
    /// <summary>
    /// Знание языков программирования
    /// </summary>
    public required List<ProgrammingLanguage> ProgrammingLanguagesKnowledge { get; init; }

    public Education? Education { get; init; }

    /// <summary>
    /// Знание языков
    /// </summary>
    public required List<LanguageKnowledge> LanguagesKnowledge { get; init; }
    
    public Country? Citizenship { get; set; }
    
    /// <summary>
    /// Разрешение на работу
    /// </summary>
    public Country? WorkPermit { get; set; }

    public override string ToString()
    {
        return $"{nameof(ResumeId)}: {ResumeId}, {nameof(PageLink)}: {PageLink}, " +
               $"{nameof(EmployeeStatus)}: {EmployeeStatus}, {nameof(Gender)}: {Gender}, {nameof(Age)}: {Age}, " +
               $"{nameof(BirthDate)}: {BirthDate}, {nameof(HasPicture)}: {HasPicture}, {nameof(Salary)}: {Salary}, " +
               $"{nameof(Title)}: {Title}, {nameof(Employments)}: {Employments}, {nameof(WorkSchedules)}: {WorkSchedules}, " +
               $"{nameof(Experience)}: {Experience}, {nameof(AboutMe)}: {AboutMe}, " +
               $"{nameof(TagList)}: {TagList}, {nameof(Education)}: {Education}, " +
               $"{nameof(LanguagesKnowledge)}: {LanguagesKnowledge}, {nameof(Citizenship)}: {Citizenship}, " +
               $"{nameof(WorkPermit)}: {WorkPermit}";
    }
}