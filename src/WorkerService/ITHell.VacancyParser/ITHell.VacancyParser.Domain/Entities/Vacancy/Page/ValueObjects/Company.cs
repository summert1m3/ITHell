namespace ITHell.VacancyParser.Domain.Entities.Vacancy.Page.ValueObjects;

public class Company
{
    public required string CompanyName { get; init; }
    
    public string? CompanyLink { get; init; }

    public string? CompanyLogoLink { get; init; }
    
    /// <summary>
    /// Пример: vacancy-serp-bage-trusted-employer (проверенный работодатель)
    /// </summary>
    public required List<string> Badges { get; init; }
    
    public decimal? CompanyRating { get; init; }
}