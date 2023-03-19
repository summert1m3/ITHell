namespace ITHell.VacancyParser.Domain.Common.Exceptions;

public class ResumePageParseFailedException : Exception
{
    public ResumePageParseFailedException(string msg) 
        : base(msg)
    {
    }
}