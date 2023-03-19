using ITHell.VacancyParser.Domain.Common;

namespace ITHell.VacancyParser.Domain.Entities.Resume.Page.ValueObjects;

public class Salary
{
    public int EstimatedSalary { get; init; }
    public Currency SalaryCurrency { get; init; }
}