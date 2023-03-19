using ITHell.VacancyParser.Domain.Common;

namespace ITHell.VacancyParser.Domain.Entities.Vacancy.Page.ValueObjects;

public class Salary
{
    public int? SalaryFrom { get; init; }
    public int? SalaryTo { get; init; }
    public Currency? SalaryCurrency { get; init; }
}