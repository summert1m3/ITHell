using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Application.Services.Parsers;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyCard;
using ITHell.VacancyParser.Domain.Entities.Vacancy.VacancyPage;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ITHell.VacancyParser.WorkerService;

public class VacancyParser : BackgroundService
{
    private readonly ILogger<VacancyParser> _logger;
    private readonly IFlareSolverrHttpClient _flareSolverrHttpClient;
    private readonly IHtmlParser _htmlParser;
    private readonly string _regionalJobSiteLink;
    private readonly string _parserStartLink;

    public VacancyParser(
        ILogger<VacancyParser> logger,
        IFlareSolverrHttpClient flareSolverrHttpClient,
        IHtmlParser htmlParser,
        IConfiguration configuration)
    {
        _logger = logger;
        _flareSolverrHttpClient = flareSolverrHttpClient;
        _htmlParser = htmlParser;

        _regionalJobSiteLink = configuration["RegionalJobSiteLink"];
        Guard.Against.NullOrWhiteSpace(_regionalJobSiteLink);

        _parserStartLink = configuration["ParserStartLink"];
        Guard.Against.NullOrWhiteSpace(_parserStartLink);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await ParseVacancyCards(_parserStartLink);

            await Task.Delay(new TimeSpan(1, 0, 0, 0), stoppingToken);
        }
    }

    void DisplayVacancyPage(VacancyPage vacancy)
    {
        Console.WriteLine($"Vacancy id: {vacancy.VacancyId}");
        Console.WriteLine($"Vacancy id: {vacancy.PageLink}");
        Console.WriteLine($"Title: {vacancy.Title}");
        Console.WriteLine($"SalaryFrom: {vacancy.Salary.SalaryFrom}");
        Console.WriteLine($"SalaryTo {vacancy.Salary.SalaryTo}");
        Console.WriteLine($"Currency {vacancy.Salary.SalaryCurrency}");
        Console.WriteLine($"ExperienceFrom: {vacancy.Experience.ExperienceFrom}");
        Console.WriteLine($"ExperienceTo: {vacancy.Experience.ExperienceTo}");

        Console.WriteLine("Work schedules:");
        foreach (var workSchedule in vacancy.WorkSchedules)
        {
            Console.WriteLine(workSchedule);
        }

        Console.WriteLine($"CompanyImgSrc: {vacancy.Company.CompanyLogoLink}");
        Console.WriteLine($"CompanyName: {vacancy.Company.CompanyName}");
        Console.WriteLine($"CompanyRating: {vacancy.Company.CompanyRating}");
        Console.WriteLine($"VacancyLocation: {vacancy.VacancyLocation}");
        Console.WriteLine($"MainContent: {vacancy.MainContent}");

        Console.WriteLine("Tag list:");
        foreach (var tag in vacancy.TagList)
        {
            Console.WriteLine(tag);
        }

        Console.WriteLine($"VacancyCreatedDate: {vacancy.VacancyCreatedDate}");
    }

    private void DisplayVacancyCard(VacancyCard vacancyCard)
    {
        Console.WriteLine($"Online users: {vacancyCard.OnlineUsers}");
        Console.WriteLine($"Vacancy page link: {vacancyCard.VacancyPageLink}");
        Console.WriteLine($"Title: {vacancyCard.Title}");
        Console.WriteLine($"Salary: {vacancyCard.Salary}");
        Console.WriteLine($"Company name: {vacancyCard.Company.CompanyName}");
        Console.WriteLine($"Company link: {vacancyCard.Company.CompanyLink}");
        Console.WriteLine($"Company logo: {vacancyCard.Company.CompanyLogoLink}");
        Console.WriteLine($"Company badges");

        foreach (var badge in vacancyCard.Company.Badges)
        {
            Console.WriteLine(badge);
        }

        Console.WriteLine($"Vacancy address: {vacancyCard.VacancyAddress}");

        Console.WriteLine($"Labels");
        foreach (var label in vacancyCard.Labels)
        {
            Console.WriteLine(label);
        }

        Console.WriteLine($"Responsibilities: {vacancyCard.VacancyDescription?.Responsibilities}");
        Console.WriteLine($"Requirements: {vacancyCard.VacancyDescription?.Requirements}");
        Console.WriteLine();
        Console.WriteLine("---------------");
        Console.WriteLine();
    }


    async Task ParseVacancyCards(string pageLink)
    {
        var pageNumber = 2;

        List<VacancyCard> allVacancyCards = new();

        while (true)
        {
            Console.WriteLine($"PAGE {pageNumber}");

            var html = await _flareSolverrHttpClient.Get(pageLink);

            var config = Configuration.Default.WithDefaultLoader();
            using var context = BrowsingContext.New(config);

            using var doc = await context.OpenAsync(req => req.Content(html));
            await doc.WaitForReadyAsync();

            var vacancyCards = _htmlParser.ParseVacancyCards(doc);

            allVacancyCards.AddRange(vacancyCards);

            foreach (var vacancyCard in vacancyCards)
            {
                DisplayVacancyCard(vacancyCard);

                await Task.Delay(new TimeSpan(0, 0, Random.Shared.Next(10, 20)));

                var vacancyPageHtml = await _flareSolverrHttpClient.Get(vacancyCard.VacancyPageLink);

                using var vacancyPageDoc
                    = await context.OpenAsync(req => req.Content(vacancyPageHtml));
                await doc.WaitForReadyAsync();

                var vacancyPage
                    = _htmlParser.ParseVacancyPage(vacancyPageDoc, vacancyCard);

                DisplayVacancyPage(vacancyPage);
            }

            Console.WriteLine(vacancyCards.Count);


            var nextLink = doc.QuerySelector("div[data-qa=\"pager-block\"] a[data-qa=\"pager-next\"]")?
                .GetAttribute("href");

            if (nextLink is null)
            {
                break;
            }

            pageLink = _regionalJobSiteLink + nextLink;

            var delay = Random.Shared.Next(15, 45);

            await Task.Delay(new TimeSpan(0, 0, delay));

            pageNumber++;
        }

        Console.WriteLine($"All vacancy cards: {allVacancyCards.Count}");
    }
}