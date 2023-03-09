namespace ITHell.VacancyParser.Domain.Entities.Resume;

public class ResumePage
{
    /// <summary>
    /// Id резюме из ссылки
    /// </summary>
    public required int ResumeId { get; init; }
    public required string PageLink { get; init; }
    public required string Title { get; init; }
    public int? Age { get; init; }
    /// <summary>
    /// Предполагаемая зарплата
    /// </summary>
    public required string Salary { get; init; }
    public required TimeSpan Experience { get; init; }
}