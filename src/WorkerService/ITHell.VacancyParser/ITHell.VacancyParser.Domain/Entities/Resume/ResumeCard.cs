namespace ITHell.VacancyParser.Domain.Entities.Resume;

public class ResumeCard
{
    /// <summary>
    /// Id резюме из ссылки
    /// </summary>
    public required int ResumeId { get; init; }
    public required string ResumePageLink { get; init; }
    public required List<string> Labels { get; init; }
}