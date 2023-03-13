namespace ITHell.VacancyParser.Domain.Common;

/// <summary>
/// Занятость
/// </summary>
public enum Employment
{
    [MultipleDescription("full time", "полная занятость")]
    FullTime,
    
    [MultipleDescription("part time", "частичная занятость")]
    PartTime,
    
    [MultipleDescription("project work", "проектная работа")]
    ProjectWork,
    
    [MultipleDescription("volunteering", "волонтерство")]
    Volunteering,
    
    [MultipleDescription("work placement", "стажировка")]
    WorkPlacement
}