using ITHell.VacancyParser.Domain.Common.Attributes;

namespace ITHell.VacancyParser.Domain.Common;

public enum Gender
{
    [MultipleDescription("Male", "Мужчина")]
    Male,
    [MultipleDescription("Female", "Женщина")]
    Female
}