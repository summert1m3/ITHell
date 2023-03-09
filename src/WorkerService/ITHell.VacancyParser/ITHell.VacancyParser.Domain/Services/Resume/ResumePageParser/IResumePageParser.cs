using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;

namespace ITHell.VacancyParser.Domain.Services.Resume.ResumePageParser;

public interface IResumePageParser
{
    public ResumePage ParseResumePageFromDom(IDocument doc, ResumeCard resumeCard);
}