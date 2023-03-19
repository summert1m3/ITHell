using ITHell.VacancyParser.Domain.Common;

namespace ITHell.VacancyParser.Domain.Entities.Resume;

public class ResumeCard : BaseEntity
{
    /// <summary>
    /// Id резюме из ссылки
    /// </summary>
    public required Guid ResumeId { get; init; }
    public required string ResumePageLink { get; init; }
    public required EmployeeStatus? EmployeeStatus { get; init; }

    public override string ToString()
    {
        return $"{nameof(ResumeId)}: {ResumeId}, {nameof(ResumePageLink)}: {ResumePageLink}, " +
               $"{nameof(EmployeeStatus)}: {EmployeeStatus}";
    }
}