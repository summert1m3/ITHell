namespace ITHell.VacancyParser.Domain.Common;

public enum Education
{
    [MultipleDescription("Secondary education", "Образование")]
    Average,
    
    [MultipleDescription("Secondary special education", "Среднее специальное образование")]
    SecondarySpecial,
    
    [MultipleDescription("Incomplete higher education", "Неоконченное высшее образование")]
    UnfinishedHigherEducation,
    
    [MultipleDescription("Higher education", "Высшее образование")]
    Higher,
    
    [MultipleDescription("Higher education (bachelor)", "Высшее образование (Бакалавр)")]
    Bachelor,
    
    [MultipleDescription("Higher education (master)", "Высшее образование (Магистр)")]
    Master,
    
    [MultipleDescription("Higher education (PhD)", "Высшее образование (Кандидат наук)")]
    PhD,
    
    [MultipleDescription("Higher education (Doctor of Science)", "Высшее образование (Доктор наук)")]
    DoctorOfSciences
}