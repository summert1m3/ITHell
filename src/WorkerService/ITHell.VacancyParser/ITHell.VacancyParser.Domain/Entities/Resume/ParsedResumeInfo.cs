using ITHell.VacancyParser.Domain.Common;
using ITHell.VacancyParser.Domain.Entities.Resume.Page;

namespace ITHell.VacancyParser.Domain.Entities.Resume;

public class ParsedResumeInfo : BaseEntity
{
    /// <summary>
    /// Поисковый запрос
    /// </summary>
    public required JobType JobType { get; init; }
    
    public required ResumeCard ResumeCard { get; init; }
    
    public required ResumePage ResumePage { get; init; }
    
    public required DateTime ParseDate { get; init; }
}