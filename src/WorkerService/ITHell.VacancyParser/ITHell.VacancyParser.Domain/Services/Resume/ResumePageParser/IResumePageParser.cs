using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.ResumePage;

namespace ITHell.VacancyParser.Domain.Services.Resume.ResumePageParser;

public interface IResumePageParser
{
    public ResumePage ParseResumePageFromDom(IDocument doc, ResumeCard resumeCard);
}