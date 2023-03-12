namespace ITHell.VacancyParser.Domain.Common;

/// <summary>
/// График работы
/// </summary>
public enum WorkSchedule
{
    [MultipleDescription("Full day", "Полный день")]
    FullDay,
    
    [MultipleDescription("Shift schedule", "Сменный график")]
    ShiftSchedule,
    
    [MultipleDescription("Flexible schedule", "Гибкий график")]
    FlexibleSchedule,
    
    [MultipleDescription("Remote working", "Удаленная работа")]
    RemoteWorking,
    
    [MultipleDescription("Rotation based work", "Вахтовый метод")]
    RotationBasedWork,
}