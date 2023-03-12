namespace ITHell.VacancyParser.Domain.Common;

/// <summary>
/// Занятость
/// </summary>
public enum Employment
{
    [MultipleDescription("Full time", "Полная занятость")]
    FullTime,
    
    [MultipleDescription("Part time", "Частичная занятость")]
    PartTime,
    
    [MultipleDescription("Project work", "Проектная работа")]
    ProjectWork,
    
    [MultipleDescription("Project work", "Волонтерство")]
    Volunteering,
    
    [MultipleDescription("Work placement", "Стажировка")]
    WorkPlacement
}