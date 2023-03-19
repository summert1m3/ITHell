using ITHell.VacancyParser.Domain.Common.Attributes;

namespace ITHell.VacancyParser.Domain.Common;

/// <summary>
/// График работы
/// </summary>
public enum WorkSchedule
{
    [MultipleDescription("full day", "полный день")]
    FullDay,
    
    [MultipleDescription("shift schedule", "сменный график")]
    ShiftSchedule,
    
    [MultipleDescription("flexible schedule", "гибкий график")]
    FlexibleSchedule,
    
    [MultipleDescription("remote working", "удаленная работа")]
    RemoteWorking,
    
    [MultipleDescription("rotation based work", "вахтовый метод")]
    RotationBasedWork,
}