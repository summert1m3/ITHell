using System.Text.RegularExpressions;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard.ValueObjects;

namespace ITHell.VacancyParser.Domain.Services.VacancyCardParser;

public class VacancyCardParser : IVacancyCardParser
{
    private readonly string _jobSiteLink;

    public VacancyCardParser(string jobSiteLink)
    {
        _jobSiteLink = jobSiteLink;
    }
    
    public List<VacancyCard> ParseVacancyCardsFromHtmlDoc(IDocument doc)
    {
        var vacancyCardEls = doc.QuerySelectorAll("div.serp-item");

        if (!vacancyCardEls.Any())
        {
            throw new Exception("Не найдено ни одной вакансии");
        }

        List<VacancyCard> vacancyCards = new();
        foreach (var vacancyEl in vacancyCardEls)
        {
            var cardHeader = vacancyEl.QuerySelector("div.vacancy-serp-item-body");
            Guard.Against.Null(cardHeader);

            var onlineUsers =
                cardHeader.QuerySelector("div.online-users--tWT3_ck7eF8Iv5SpZ6WL")?.TextContent;

            var titleEl = cardHeader.QuerySelector("a.serp-item__title[data-qa=\"serp-item__title\"]");
            Guard.Against.Null(titleEl);

            var vacancyPageLink = titleEl.GetAttribute("href");
            Guard.Against.Null(vacancyPageLink);
            
            var title = titleEl?.TextContent;
            Guard.Against.Null(title);

            var salary = cardHeader.QuerySelector("span[data-qa=\"vacancy-serp__vacancy-compensation\"]")?.TextContent;
            if (salary is null)
            {
                Console.WriteLine("Зарплата не найдена");
            }

            var companyInfo = ParseCompanyInfo(cardHeader);

            var vacancyLabels = ParseVacancyLabels(vacancyEl);

            var vacancyDescription = ParseVacancyBody(vacancyEl);

            vacancyCards.Add(new VacancyCard()
            {
                //Выбирает первый int из строки
                VacancyId = Regex.Matches(vacancyPageLink, @"\d+")
                    .Cast<Match>()
                    .Select(m => int.Parse(m.Value))
                    .ToList()[0],
                OnlineUsers = onlineUsers,
                VacancyPageLink = vacancyPageLink,
                Title = title,
                Salary = salary,
                Company = new Company()
                {
                    CompanyName = companyInfo.CompanyName,
                    CompanyLink = companyInfo.CompanyLink,
                    CompanyLogoLink = companyInfo.CompanyLogoLink,
                    Badges = companyInfo.Badges
                },
                VacancyAddress = companyInfo.VacancyAddress,
                Labels = vacancyLabels,
                VacancyDescription = vacancyDescription
            });
        }

        return vacancyCards;
    }

    private (string CompanyName, string? CompanyLink, 
        string? CompanyLogoLink, List<string> Badges, string VacancyAddress)
        ParseCompanyInfo(IElement cardHeader)
    {
        var companyInfoEl = cardHeader.QuerySelector("div.vacancy-serp-item-company");
        Guard.Against.Null(companyInfoEl);

        var companyMetaInfoElement = companyInfoEl.QuerySelector("div.vacancy-serp-item__meta-info-company");
        Guard.Against.Null(companyMetaInfoElement);
        
        var companyNameElement = companyMetaInfoElement
            .QuerySelector("a[data-qa=\"vacancy-serp__vacancy-employer\"]");

        string companyName;
        string? companyLink = null;
        if (companyNameElement is not null)
        {
            companyName = companyNameElement.TextContent;
            Guard.Against.NullOrWhiteSpace(companyName);

            companyLink = _jobSiteLink + companyNameElement.GetAttribute("href");
            Guard.Against.Null(companyLink);
        }
        else
        {
            Console.WriteLine("Не найдена ссылка компании");
            
            companyName = companyMetaInfoElement.TextContent;
            Guard.Against.NullOrWhiteSpace(companyName);
        }

        List<string> ParseBadges(IElement companyInfoEl)
        {
            List<string> badges = new();

            var badgesEl = companyInfoEl.QuerySelector("div.vacancy-serp-item__meta-info-badges");
            if (badgesEl is null)
            {
                Console.WriteLine("Не найдено Badges для вакансии");

                return badges;
            }

            var badgesEls = badgesEl.QuerySelectorAll("div.vacancy-serp-item__meta-info-link");

            if (!badgesEls.Any())
            {
                Console.WriteLine("Не найдено Badges для вакансии");
            }

            foreach (var badgeEl in badgesEls)
            {
                var badge = badgeEl.QuerySelector("a > span")?.ClassName;
                if (badge is null)
                {
                    continue;
                }
                Guard.Against.NullOrWhiteSpace(badge);

                badges.Add(badge);
            }

            return badges;
        }

        var badges = ParseBadges(companyInfoEl);

        var vacancyAddress = companyInfoEl.QuerySelector("div[data-qa=\"vacancy-serp__vacancy-address\"]")
            ?.TextContent;
        Guard.Against.Null(vacancyAddress);

        var companyLogoLink =
            cardHeader.QuerySelector(
                "div.vacancy-serp-item-control-gt-xs div.vacancy-serp-item-body__logo img")?
                .GetAttribute("src");

        return (companyName, companyLink, companyLogoLink, badges, vacancyAddress);
    }

    private VacancyDescription? ParseVacancyBody(IElement vacancyEl)
    {
        var vacancyBody = vacancyEl.QuerySelector("div.vacancy-serp-item__info > div.g-user-content");
        if (vacancyBody is null)
        {
            return null;
        }

        var responsibility =
            vacancyBody.QuerySelector("div[data-qa=\"vacancy-serp__vacancy_snippet_responsibility\"]")?.TextContent;
        if (responsibility is null)
        {
            Console.WriteLine("Ответственности не найдены");
        }

        var requirements =
            vacancyBody.QuerySelector("div[data-qa=\"vacancy-serp__vacancy_snippet_requirement\"]")?.TextContent;
        if (requirements is null)
        {
            Console.WriteLine("Требования не найдены");
        }

        return new VacancyDescription()
        {
            Responsibilities = responsibility,
            Requirements = requirements
        };
    }

    private List<string> ParseVacancyLabels(IElement vacancyEl)
    {
        var vacancyLabelsElements = vacancyEl.QuerySelectorAll("div.vacancy-serp-item__label");
        if (!vacancyLabelsElements.Any())
        {
            Console.WriteLine("Не найдено Labels для вакансии");
        }

        List<string> vacancyLabels = new();

        foreach (var labelEl in vacancyLabelsElements)
        {
            var label = labelEl.QuerySelector("div.bloko-text")?.TextContent;
            Guard.Against.Null(label);

            vacancyLabels.Add(label);
        }

        return vacancyLabels;
    }
}