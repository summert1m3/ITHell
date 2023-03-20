using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Application.Services.Parsers;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Card;
using ITHell.VacancyParser.Domain.Entities.Vacancy.Page;
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

        _parserStartLink = configuration["VacancyInitialPage"];
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
        Console.WriteLine();

        Console.WriteLine(vacancy);
        
        Console.WriteLine();
    }

    private void DisplayVacancyCard(VacancyCard vacancyCard)
    {
        Console.WriteLine();

        Console.WriteLine(vacancyCard);
        
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

                await Task.Delay(new TimeSpan(0, 0, Random.Shared.Next(5, 15)));

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

            var delay = Random.Shared.Next(15, 35);

            await Task.Delay(new TimeSpan(0, 0, delay));

            pageNumber++;
        }

        Console.WriteLine($"All vacancy cards: {allVacancyCards.Count}");
    }
}