using AngleSharp.Dom;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.Page;

namespace ITHell.VacancyParser.Domain.Services.Resume.PageParser;

public interface IResumePageParser
{
    public ResumePage ParseResumePageFromDom(IDocument doc, ResumeCard resumeCard);
}