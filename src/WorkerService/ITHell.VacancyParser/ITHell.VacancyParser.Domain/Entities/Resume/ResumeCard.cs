namespace ITHell.VacancyParser.Domain.Entities.Resume;

public class ResumeCard
{
    /// <summary>
    /// Id резюме из ссылки
    /// </summary>
    public required Guid ResumeId { get; init; }
    public required string ResumePageLink { get; init; }
    public required EmployeeStatus? EmployeeStatus { get; init; }
}