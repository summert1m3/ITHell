using System.Globalization;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Domain.Common;
using ITHell.VacancyParser.Domain.Common.Exceptions;
using ITHell.VacancyParser.Domain.Common.Language;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.Page;
using ITHell.VacancyParser.Domain.Entities.Resume.Page.ValueObjects;

namespace ITHell.VacancyParser.Domain.Services.Resume.PageParser;

public class ResumePageParser : IResumePageParser
{
    public ResumePage ParseResumePageFromDom(IDocument doc, ResumeCard resumeCard)
    {
        try
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

            var progLangKnowlg = GetProgrammingLanguagesKnowledge(tags);

            var aboutMe = mainContent
                .QuerySelector("div[data-qa=\"resume-block-skills-content\"]")?
                .TextContent;

            var education = ParseEducation(mainContent);

            var languages = ParseLanguages(mainContent);

            var citizenshipEls = mainContent
                .QuerySelectorAll(
                    "div[data-qa=\"resume-block-additional\"] div.resume-block-container p");

            var citizenship = ParseCitizenship(citizenshipEls);

            var workPermit = ParseWorkPermit(citizenshipEls);

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
                ProgrammingLanguagesKnowledge = progLangKnowlg,
                Education = education,
                LanguagesKnowledge = languages,
                Citizenship = citizenship,
                WorkPermit = workPermit
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Не удалось спарсить страницу резюме:\n{e.Message}\n{resumeCard.ResumePageLink}");
            throw;
        }
    }

    private static List<ProgrammingLanguage> GetProgrammingLanguagesKnowledge(List<string> tags)
    {
        List<ProgrammingLanguage> programmingLanguages = new();
        
        foreach (var tag in tags)
        {
            try
            {
                var progLang = EnumParser.ParseEnumMultipleDescription<ProgrammingLanguage>(tag);
                
                programmingLanguages.Add(progLang);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return programmingLanguages;
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

        var workSchedulesClear = workSchedulesRaw.Substring(
            workSchedulesRaw.IndexOf(": ", StringComparison.Ordinal) + 2);

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
                // ignored
            }
        }

        return workSchedules;
    }

    private static Education? ParseEducation(IElement mainContent)
    {
        var educationStr = mainContent
            .QuerySelector(
                "div[data-qa=\"resume-block-education\"] span.resume-block__title-text.resume-block__title-text_sub")?
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
                "div[data-qa=\"resume-block-experience\"] span.resume-block__title-text.resume-block__title-text_sub")?
            .TextContent;

        TimeSpan? experience = null;

        if (experienceStr is not null)
        {
            var experienceStrParsed = experienceStr.Replace('\u00A0', ' ');

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

            if (salaryStr.Contains("руб.") || salaryStr.Contains('₽'))
            {
                salaryCurrency = Currency.RUB;
            }
            else if (salaryStr.Contains("USD") || salaryStr.Contains('$'))
            {
                salaryCurrency = Currency.USD;
            }
            else if (salaryStr.Contains("EUR") || salaryStr.Contains('€'))
            {
                salaryCurrency = Currency.EUR;
            }
            else if (salaryStr.Contains("KZT") || salaryStr.Contains('₸'))
            {
                salaryCurrency = Currency.KZT;
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

        var workPermitParsed = workPermitRaw.Substring(
            workPermitRaw.IndexOf(": ", StringComparison.Ordinal) + 2);

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

    private static Country? ParseCitizenship(IHtmlCollection<IElement> citizenshipEls)
    {
        Country? citizenship = null;

        var citizenshipRaw = citizenshipEls
            .SingleOrDefault(x => x.TextContent.Contains("Гражданство")
                                  || x.TextContent.Contains("Citizenship"))?
            .TextContent;

        if (citizenshipRaw is null) return citizenship;

        var citizenshipParsed = citizenshipRaw.Substring(citizenshipRaw.IndexOf(": ") + 2);

        try
        {
            citizenship = EnumParser.ParseEnumMultipleDescription<Country>(citizenshipParsed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return citizenship;
    }

    private static List<LanguageKnowledge> ParseLanguages(IElement mainContent)
    {
        var languagesEl = mainContent
            .QuerySelectorAll(
                "div[data-qa=\"resume-block-languages\"] div.bloko-tag-list > div");

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
                    try
                    {
                        Language lang = EnumParser.ParseEnumMultipleDescription<Language>(langParsed[0]);
                        languagesKnowledge.Add(
                            new LanguageKnowledge()
                            {
                                Language = lang,
                                LanguageLevel = LanguageLevel.Native
                            });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
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

        var tagListEl 
            = tagsEl.QuerySelectorAll("div.bloko-tag.bloko-tag_inline.bloko-tag_countable");

        foreach (var tagEl in tagListEl)
        {
            var tag = tagEl.QuerySelector("span")?.TextContent;

            if (tag is null)
            {
                throw new ResumePageParseFailedException("tag is null");
            }

            tagList.Add(tag);
        }

        return tagList;
    }

    private static TimeSpan ParseTimeSpanFromStr(string timeString)
    {
        int years = 0;
        int months = 0;
        string[] parts = timeString.Split(' ');

        for (int i = 2; i < parts.Length; i += 2)
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

    private (Gender Gender, int? Age, DateTime? BirthDay, bool HasPhoto)
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

        int? age = null;
        if (!string.IsNullOrWhiteSpace(ageStr))
        {
            age = int.Parse(new string(ageStr.Where(char.IsDigit).ToArray()));
        }

        var birthDayStr = header
            .QuerySelector("span[data-qa=\"resume-personal-birthday\"]")?
            .TextContent
            .Replace('\u00A0', ' ');

        DateTime? birthDay = null;

        if (!string.IsNullOrWhiteSpace(birthDayStr))
        {
            birthDay = ParseBirthDay(birthDayStr);
        }

        var photoEl = header.QuerySelector("div[data-qa=\"resume-photo-forbidden\"]");

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