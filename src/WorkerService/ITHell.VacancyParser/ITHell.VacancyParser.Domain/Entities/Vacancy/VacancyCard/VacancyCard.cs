using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard.ValueObjects;

namespace ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;

public class VacancyCard
{
    /// <summary>
    /// Id вакансии из ссылки
    /// </summary>
    public required int VacancyId { get; init; }
    public required string? OnlineUsers { get; init; }
    
    public required string VacancyPageLink { get; init; }
    
    public required string Title { get; init; }
    
    /// <summary>
    /// Зарплата, может быть в диапазоне
    /// </summary>
    public required string? Salary { get; init; }
    
    public required Company Company { get; init; }
    
    public required string VacancyAddress { get; init; }
    
    /// <summary>
    /// Пример: Можно из дома
    /// </summary>
    public required List<string> Labels { get; init; }

    public required VacancyDescription? VacancyDescription { get; init; }
}