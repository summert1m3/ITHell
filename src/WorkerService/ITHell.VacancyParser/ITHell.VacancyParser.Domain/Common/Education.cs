namespace ITHell.VacancyParser.Domain.Common;

public enum Education
{
    [MultipleDescription("Secondary", "Среднее")]
    Average,
    
    [MultipleDescription("Vocational secondary", "Среднее специальное")]
    SecondarySpecial,
    
    [MultipleDescription("Incomplete higher education", "Неоконченное высшее")]
    UnfinishedHigherEducation,
    
    [MultipleDescription("Higher", "Высшее")]
    Higher,
    
    [MultipleDescription("Bachelor", "Бакалавр")]
    Bachelor,
    
    [MultipleDescription("Master", "Магистр")]
    Master,
    
    [MultipleDescription("PhD", "Кандидат наук")]
    PhD,
    
    [MultipleDescription("Doctor of Sciences", "Доктор наук")]
    DoctorOfSciences
}