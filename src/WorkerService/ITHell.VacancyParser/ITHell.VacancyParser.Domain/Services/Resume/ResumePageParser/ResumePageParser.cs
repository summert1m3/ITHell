using System.Globalization;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Domain.Common;
using ITHell.VacancyParser.Domain.Common.Language;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.ResumePage;
using ITHell.VacancyParser.Domain.Entities.Resume.ResumePage.ValueObjects;

namespace ITHell.VacancyParser.Domain.Services.Resume.ResumePageParser;

public class ResumePageParser : IResumePageParser
{
    public ResumePage ParseResumePageFromDom(IDocument doc, ResumeCard resumeCard)
    {
        var mainContent = doc.QuerySelector("div.main-content div.resume-applicant");
        Guard.Against.Null(mainContent);

        var header = ParseHeader(mainContent);

        var resumeMain = mainContent.QuerySelector("div.resume-wrapper");
        Guard.Against.Null(resumeMain);

        var title = resumeMain.QuerySelector(
                "span[data-qa=\"resume-block-title-position\"]")?
            .TextContent;
        Guard.Against.NullOrWhiteSpace(title);

        var salary = ParseSalary(mainContent);

        var elements = mainContent
            .QuerySelectorAll("div.resume-block-item-gap div.resume-block-container p");
        var employments = ParseEmployments(elements[0]);
        var workSchedules = ParseWorkSchedules(elements[1]);
        
        var experience = ParseExperience(mainContent);

        var tags = ParseTagList(mainContent);

        var aboutMe = mainContent
            .QuerySelector("div[data-qa=\"resume-block-skills-content\"]")?
            .TextContent;

        var education = ParseEducation(mainContent);

        var languages = ParseLanguages(mainContent);

        var citezenshipEls = mainContent
            .QuerySelectorAll(
                "div[data-qa=\"resume-block-additional\"] div.resume-block-container p");

        var citezenship = ParseCitezenship(citezenshipEls);

        var workPermit = ParseWorkPermit(citezenshipEls);

        return new ResumePage()
        {
            ResumeId = resumeCard.ResumeId,
            PageLink = resumeCard.ResumePageLink,
            EmployeeStatus = resumeCard.EmployeeStatus,
            Gender = header.Gender,
            Age = header.Age,
            BirthDate = header.BirthDay,
            HasPicture = header.HasPhoto,
            Salary = salary,
            Title = title,
            Employments = employments,
            WorkSchedules = workSchedules,
            Experience = experience,
            AboutMe = aboutMe,
            TagList = tags,
            Education = education,
            LanguagesKnowledge = languages,
            Citizenship = citezenship,
            WorkPermit = workPermit
        };
    }

    private static List<Employment> ParseEmployments(IElement element)
    {
        var employmentsRaw = element.TextContent;
        var employmentsClear = employmentsRaw.Remove(0, 11);
        var employmentsStr = employmentsClear.Split(", ");

        List<Employment> employments = new();

        foreach (var emplStr in employmentsStr)
        {
            try
            {
                var parsedEmpl = EnumParser.ParseEnumMultipleDescription<Employment>(emplStr);
                employments.Add(parsedEmpl);
            }
            catch (Exception e)
            {
                continue;
            }
        }

        return employments;
    }

    private static List<WorkSchedule> ParseWorkSchedules(IElement element)
    {
        var workSchedulesRaw = element.TextContent;
        var workSchedulesClear = workSchedulesRaw.Remove(0, 11);
        var workSchedulesStr = workSchedulesClear.Split(", ");

        List<WorkSchedule> workSchedules = new();

        foreach (var workSchStr in workSchedulesStr)
        {
            try
            {
                var parsedWorkSch = EnumParser.ParseEnumMultipleDescription<WorkSchedule>(workSchStr);
                workSchedules.Add(parsedWorkSch);
            }
            catch (Exception e)
            {
                continue;
            }
        }

        return workSchedules;
    }

    private static Education? ParseEducation(IElement mainContent)
    {
        var educationStr = mainContent
            .QuerySelector("div[data-qa=\"resume-block-education\"]")?
            .TextContent;

        Education? education = null;

        if (educationStr is not null)
        {
            education = EnumParser.ParseEnumMultipleDescription<Education>(educationStr);
        }

        return education;
    }

    private static TimeSpan? ParseExperience(IElement mainContent)
    {
        var experienceStr = mainContent
            .QuerySelector(
                "div[data-qa=\"resume-block-experience\"] span.resume-block__title-text resume-block__title-text_sub")?
            .TextContent;

        TimeSpan? experience = null;

        if (experienceStr is not null)
        {
            var experienceStrParsed = experienceStr.Remove(0, 12);

            experience = ParseTimeSpanFromStr(experienceStrParsed);
        }

        return experience;
    }

    private static Salary? ParseSalary(IElement resumeMain)
    {
        var salaryStr = resumeMain
            .QuerySelector("span[data-qa=\"resume-block-salary\"]")?
            .TextContent;

        Salary? salary = null;

        if (salaryStr is not null)
        {
            int estimatedSalary = int.Parse(new string(salaryStr.Where(char.IsDigit).ToArray()));
            Currency? salaryCurrency = null;

            if (salaryStr.Contains("руб."))
            {
                salaryCurrency = Currency.RUB;
            }
            else if (salaryStr.Contains("USD"))
            {
                salaryCurrency = Currency.USD;
            }
            else if (salaryStr.Contains("EUR"))
            {
                salaryCurrency = Currency.EUR;
            }

            Guard.Against.Null(salaryCurrency);

            salary = new Salary()
            {
                EstimatedSalary = estimatedSalary,
                SalaryCurrency = (Currency)salaryCurrency
            };
        }

        return salary;
    }

    private static Country? ParseWorkPermit(IHtmlCollection<IElement> citezenshipEls)
    {
        Country? workPermit = null;
        
        var workPermitRaw = citezenshipEls
            .SingleOrDefault(x => x.TextContent.Contains("Разрешение на работу") 
                         || x.TextContent.Contains("Permission to work"))?
            .TextContent;

        if (workPermitRaw is null) return workPermit;
        
        var workPermitParsed = workPermitRaw.Substring(workPermitRaw.IndexOf(": ") + 2);
        
        try
        {
            workPermit = EnumParser.ParseEnumMultipleDescription<Country>(workPermitParsed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return workPermit;
    }
    
    private static Country? ParseCitezenship(IHtmlCollection<IElement> citezenshipEls)
    {
        Country? citezenship = null;
        
        var citezenshipRaw = citezenshipEls
            .SingleOrDefault(x => x.TextContent.Contains("Гражданство") 
                         || x.TextContent.Contains("Citizenship"))?
            .TextContent;

        if (citezenshipRaw is null) return citezenship;
        
        var citezenshipParsed = citezenshipRaw.Substring(citezenshipRaw.IndexOf(": ") + 2);
        
        try
        {
            citezenship = EnumParser.ParseEnumMultipleDescription<Country>(citezenshipParsed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return citezenship;
    }

    private static List<LanguageKnowledge> ParseLanguages(IElement mainContent)
    {
        var languagesEl = mainContent
            .QuerySelectorAll(
                "div[data-qa=\"resume-block-languages\"] div.bloko-tag-list div");

        List<LanguageKnowledge> languagesKnowledge = new();

        foreach (var languageEl in languagesEl)
        {
            var langRaw = languageEl.QuerySelector(
                "p[data-qa=\"resume-block-language-item\"]")?.TextContent;
            Guard.Against.NullOrWhiteSpace(langRaw);

            var langParsed = langRaw.Split(" — ");

            switch (langParsed.Length)
            {
                case 2:
                {
                    Language lang = EnumParser.ParseEnumMultipleDescription<Language>(langParsed[0]);
                    languagesKnowledge.Add(
                        new LanguageKnowledge()
                        {
                            Language = lang,
                            LanguageLevel = LanguageLevel.Native
                        });
                    break;
                }
                case 3:
                    try
                    {
                        Language lang = EnumParser.ParseEnumMultipleDescription<Language>(langParsed[0]);
                        LanguageLevel langLevel
                            = EnumParser.ParseEnumMultipleDescription<LanguageLevel>(langParsed[1]);

                        languagesKnowledge.Add(
                            new LanguageKnowledge()
                            {
                                Language = lang,
                                LanguageLevel = langLevel
                            });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        continue;
                    }

                    break;
                default:
                    throw new Exception("langParsed.Length != 3 && != 2");
            }
        }

        return languagesKnowledge;
    }

    /// <summary>
    /// Ключевые навыки
    /// </summary>
    private static List<string> ParseTagList(IElement main)
    {
        var tagList = new List<string>();

        var tagsEl =
            main.QuerySelector("div[data-qa=\"skills-table\"] div.bloko-tag-list");

        if (tagsEl is null)
        {
            Console.WriteLine("Ключевые навыки не найдены");

            return tagList;
        }

        var tagListEl = tagsEl.QuerySelectorAll("div");

        foreach (var tagEl in tagListEl)
        {
            var tag = tagEl.QuerySelector("span")?.TextContent;
            Guard.Against.NullOrWhiteSpace(tag);

            tagList.Add(tag);
        }

        return tagList;
    }

    private static TimeSpan ParseTimeSpanFromStr(string timeString)
    {
        int years = 0;
        int months = 0;
        string[] parts = timeString.Split(' ');

        for (int i = 0; i < parts.Length; i += 2)
        {
            int value = int.Parse(parts[i]);
            switch (parts[i + 1])
            {
                case "years":
                case "year":
                case "года":
                case "год":
                case "лет":
                    years = value;
                    break;
                case "months":
                case "month":
                case "месяцев":
                case "месяц":
                case "месяца":
                    months = value;
                    break;
                default:
                    throw new ArgumentException("Invalid time string format.");
            }
        }

        return new TimeSpan(years * 365 + months * 30, 0, 0, 0);
    }

    private (Gender Gender, int Age, DateTime BirthDay, bool HasPhoto)
        ParseHeader(IElement mainContent)
    {
        var header = mainContent.QuerySelector(
            "div.resume-header div.resume-header-main div.resume-header-title");
        Guard.Against.Null(header);

        var genderStr = header.QuerySelector("span[data-qa=\"resume-personal-gender\"]")?
            .TextContent;
        Guard.Against.NullOrWhiteSpace(genderStr);

        Gender gender = EnumParser.ParseEnumMultipleDescription<Gender>(genderStr);

        var ageStr = header.QuerySelector("span[data-qa=\"resume-personal-age\"]")?
            .TextContent;
        Guard.Against.NullOrWhiteSpace(ageStr);

        var age = int.Parse(new string(ageStr.Where(char.IsDigit).ToArray()));

        var birthDayStr = header
            .QuerySelector("span[data-qa=\"resume-personal-birthday\"]")?
            .TextContent
            .Replace('\u00A0', ' ');
        Guard.Against.NullOrWhiteSpace(birthDayStr);

        var birthDay = ParseBirthDay(birthDayStr);

        var photoEl = header.QuerySelector("div.resume-header-photo-desktop");

        bool hasPhoto;

        hasPhoto = photoEl is not null;

        return (gender, age, birthDay, hasPhoto);
    }

    private DateTime ParseBirthDay(string birthDayStr)
    {
        if (DateTime.TryParseExact(birthDayStr, "d MMMM yyyy",
                System.Globalization.CultureInfo.GetCultureInfo("ru-RU"),
                DateTimeStyles.None, out var birthDayRu))
        {
            return birthDayRu;
        }

        if (DateTime.TryParseExact(birthDayStr, "d MMMM yyyy",
                System.Globalization.CultureInfo.GetCultureInfo("en-US"),
                DateTimeStyles.None, out var birthDayEn))
        {
            return birthDayEn;
        }

        throw new Exception("Birthday parse fail");
    }
}