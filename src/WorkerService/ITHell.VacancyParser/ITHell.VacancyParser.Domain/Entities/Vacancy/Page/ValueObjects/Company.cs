namespace ITHell.VacancyParser.Domain.Entities.Vacancy.Page.ValueObjects;

public class Company
{
    public required string CompanyName { get; init; }
    
    public required string? CompanyLink { get; init; }

    public required string? CompanyLogoLink { get; init; }
    
    /// <summary>
    /// Пример: vacancy-serp-bage-trusted-employer (проверенный работодатель)
    /// </summary>
    public required List<string> Badges { get; init; }
    
    public decimal? CompanyRating { get; set; }
}