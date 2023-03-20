using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Domain.Common;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Card;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Page;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Page.ValueObjects;

namespace ITHell.VacancyParser.Domain.Services.Vacancy.PageParser;

public class VacancyPageParser : IVacancyPageParser
{
    public VacancyPage ParseVacancyPageFromDom(IDocument doc, VacancyCard vacancyCard)
    {
        var mainContent = doc.QuerySelector("div.main-content");
        Guard.Against.Null(mainContent);

        var header = ParseHeader(mainContent);

        var company = ParseCompany(mainContent);

        var vacancyMainContent = ParseMainContent(mainContent);

        var tagList = ParseTagList(mainContent);
        
        var progLangKnowlg = GetProgrammingLanguagesKnowledge(tagList);

        var vacancyCreatedDate = ParseVacancyCreatedDate(mainContent);

        return new VacancyPage()
        {
            VacancyId = vacancyCard.VacancyId,
            PageLink = vacancyCard.VacancyPageLink,
            Title = header.Title,
            
            Salary = new Salary()
            {
                SalaryFrom = header.SalaryFrom,
                SalaryTo = header.SalaryTo,
                SalaryCurrency = header.salaryCurrency,
            },
            
            Experience = new Experience()
            {
                ExperienceFrom = header.ExperienceFrom,
                ExperienceTo = header.ExperienceTo,
            },
            WorkSchedules = header.WorkSchedules,
            
            Company = new Company()
            {
                CompanyLogoLink = company.CompanyImgSrc,
                CompanyName = company.ComapnyName,
                CompanyLink = vacancyCard.Company.CompanyLink,
                Badges = vacancyCard.Company.Badges,
                CompanyRating = company.CompanyRating,
            },
            VacancyLocation = company.VacancyLocation,

            MainContent = vacancyMainContent,

            TagList = tagList,
            
            ProgrammingLanguagesKnowledge = progLangKnowlg,

            VacancyCreatedDate = vacancyCreatedDate
        };
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

    private static
        (string Title, int? SalaryFrom, int? SalaryTo, Currency? salaryCurrency,
        int? ExperienceFrom, int? ExperienceTo, List<WorkSchedule> WorkSchedules)
        ParseHeader(IElement main)
    {
        var header = main.QuerySelector("div.wrapper-flat--H4DVL_qLjKLCo1sytcNI");
        
        header ??= main.QuerySelector("div.vacancy-photo-top__shadow");
        
        Guard.Against.Null(header);

        var title = header.QuerySelector("h1[data-qa=\"vacancy-title\"]")?.TextContent;
        Guard.Against.NullOrWhiteSpace(title);

        int? salaryFrom = null;
        int? salaryTo = null;

        Currency? salaryCurrency = null;

        var emptySalary = header
            .QuerySelector("span[data-qa=\"vacancy-salary-compensation-type-undefined\"]")?.TextContent;

        if (emptySalary is null)
        {
            //Зарплата в интервале
            var salaryInterval = header
                                     .QuerySelector("span[data-qa=\"vacancy-salary-compensation-type-net\"]")
                                     ?.TextContent
                                 ??
                                 header.QuerySelector(
                                     "span[data-qa=\"vacancy-salary-compensation-type-gross\"]")?.TextContent;
            Guard.Against.Null(salaryInterval);

            var salaryIntervalNoWhiteSpaces = salaryInterval
                .Replace('\u00A0', ' ').Replace(" ", "");

            var range = Regex.Matches(salaryIntervalNoWhiteSpaces, @"\d+")
                .Cast<Match>()
                .Select(m => int.Parse(m.Value))
                .ToList();

            switch (range.Count)
            {
                case > 2:
                    throw new Exception("salary range.Count > 2");
                case 1 when salaryIntervalNoWhiteSpaces.Contains("от"):
                    salaryFrom = range[0];
                    break;
                case 1 when salaryIntervalNoWhiteSpaces.Contains("до"):
                {
                    salaryTo = range[0];
                    break;
                }
                default:
                    salaryFrom = range[0];
                    salaryTo = range[1];
                    break;
            }

            if (salaryIntervalNoWhiteSpaces.Contains("руб."))
            {
                salaryCurrency = Currency.RUB;
            }
            else if (salaryIntervalNoWhiteSpaces.Contains("USD"))
            {
                salaryCurrency = Currency.USD;
            }
            else if (salaryIntervalNoWhiteSpaces.Contains("EUR"))
            {
                salaryCurrency = Currency.EUR;
            }
            else if(salaryIntervalNoWhiteSpaces.Contains("KZT"))
            {
                salaryCurrency = Currency.KZT;
            }
        }

        int? experienceFrom = null;
        int? experienceTo = null;

        var experience = header.QuerySelector("span[data-qa=\"vacancy-experience\"]")?.TextContent;
        Guard.Against.NullOrWhiteSpace(experience);

        var experienceRange = Regex.Matches(experience, @"\d+")
            .Cast<Match>()
            .Select(m => int.Parse(m.Value))
            .ToList();

        switch (experienceRange.Count)
        {
            case > 2:
                throw new Exception("salary range.Count > 2");
            case 1:
                experienceFrom = experienceRange[0];
                break;
            case 0:
                experienceFrom = 0;
                experienceTo = 0;
                break;
            default:
                experienceFrom = experienceRange[0];
                experienceTo = experienceRange[1];
                break;
        }

        var workSchedules = ParseWorkSchedules(header);

        return (title, salaryFrom, salaryTo, salaryCurrency, experienceFrom, experienceTo, workSchedules);
    }

    private static List<WorkSchedule> ParseWorkSchedules(IElement header)
    {
        var workSchedulesString = header
            .QuerySelector("p[data-qa=\"vacancy-view-employment-mode\"]")?.TextContent;
        Guard.Against.NullOrWhiteSpace(workSchedulesString);

        var workSchedulesStr = workSchedulesString
            .Split(',').Select(x => x.Trim()).ToList();
        
        List<WorkSchedule> workSchedules = new();

        foreach (var workScheduleStr in workSchedulesStr)
        {
            try
            {
                var parsedWorkSch = EnumParser
                    .ParseEnumMultipleDescription<WorkSchedule>(workScheduleStr);
                workSchedules.Add(parsedWorkSch);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return workSchedules;
    }

    private static
        (string? CompanyImgSrc, string ComapnyName, decimal? CompanyRating, string? VacancyLocation)
        ParseCompany(IElement main)
    {
        var companyEl = main.QuerySelector("div[data-qa=\"vacancy-company\"]");
        Guard.Against.Null(companyEl);

        var companyImgSrc = companyEl
            .QuerySelector("img.vacancy-company-logo-image-redesigned")?.GetAttribute("src");

        var companyName = companyEl
            .QuerySelector(
                "span.vacancy-company-name > span, a[data-qa=\"vacancy-company-name\"] > span")?.TextContent;
        Guard.Against.NullOrWhiteSpace(companyName);

        decimal? companyRatingDecimal = null;
        var companyRating = companyEl.QuerySelector("div.AjI0Ncv___rating-container div")?.TextContent;
        
        if (companyRating is not null)
        {
            companyRatingDecimal = decimal.Parse(companyRating);
        }

        string? vacancyLocation;

        var vacancyLocationEl = companyEl.QuerySelector("p[data-qa=\"vacancy-view-location\"]");
        if (vacancyLocationEl is not null)
        {
            vacancyLocation = vacancyLocationEl.TextContent;
        }
        else
        {
            var vacancyLocationRaw
                = companyEl.QuerySelector("span[data-qa=\"vacancy-view-raw-address\"]");

            vacancyLocation = vacancyLocationRaw?.TextContent;
        }

        return (companyImgSrc, companyName, companyRatingDecimal, vacancyLocation);
    }

    private static string ParseMainContent(IElement main)
    {
        var vacancyBodyRawText
            = main.QuerySelector("div[data-qa=\"vacancy-description\"]")?.TextContent;
        Guard.Against.NullOrWhiteSpace(vacancyBodyRawText);

        return vacancyBodyRawText;
    }

    /// <summary>
    /// Ключевые навыки
    /// </summary>
    private static List<string> ParseTagList(IElement main)
    {
        var tagList = new List<string>();

        var tagsEl =
            main.QuerySelector("div.bloko-tag-list");

        if (tagsEl is null)
        {
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

    private static DateTime ParseVacancyCreatedDate(IElement main)
    {
        var createdDateEl = 
            main.QuerySelector("p.vacancy-creation-time-redesigned") 
            ?? 
            main.QuerySelector("p.vacancy-creation-time");

        Guard.Against.Null(createdDateEl);

        return ParseTime(createdDateEl.TextContent);
    }

    private static DateTime ParseTime(string input)
    {
        string[] words = input.Split(' ');
        string dateString = words[2].Replace('\u00A0', ' ');
        
        DateTime date = DateTime.ParseExact(dateString, "d MMMM yyyy",
            System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));

        return date;
    }
}