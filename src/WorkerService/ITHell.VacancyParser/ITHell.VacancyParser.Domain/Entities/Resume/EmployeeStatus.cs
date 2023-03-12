using System.ComponentModel;

namespace ITHell.VacancyParser.Domain.Entities.Resume;

public enum EmployeeStatus
{
    [Description("Активно ищет работу")]
    Active,
    [Description("Не ищет работу")]
    Inactive,
    [Description("Рассматривает предложения")]
    ConsidersOffers,
    [Description("Предложили работу, решает")]
    ReceivedOffer,
    [Description("Вышел на новое место")]
    FoundJob
}