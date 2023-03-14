namespace ITHell.VacancyParser.Domain.Common;

public enum Education
{
    [MultipleDescription("Secondary", "Образование")]
    Average,
    
    [MultipleDescription("Vocational secondary", "Среднее специальное образование")]
    SecondarySpecial,
    
    [MultipleDescription("Incomplete higher education", "Неоконченное высшее образование")]
    UnfinishedHigherEducation,
    
    [MultipleDescription("Higher", "Высшее образование")]
    Higher,
    
    [MultipleDescription("Bachelor", "Высшее образование (Бакалавр)")]
    Bachelor,
    
    [MultipleDescription("Master", "Высшее образование (Магистр)")]
    Master,
    
    [MultipleDescription("PhD", "Высшее образование (Кандидат наук)")]
    PhD,
    
    [MultipleDescription("Doctor of Sciences", "Высшее образование (Доктор наук)")]
    DoctorOfSciences
}