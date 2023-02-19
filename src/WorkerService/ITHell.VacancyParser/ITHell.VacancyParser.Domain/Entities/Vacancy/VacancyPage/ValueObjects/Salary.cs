namespace ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage.ValueObjects;

public class Salary
{
    public int? SalaryFrom { get; init; }
    public int? SalaryTo { get; init; }
    public Currency? SalaryCurrency { get; init; }
}