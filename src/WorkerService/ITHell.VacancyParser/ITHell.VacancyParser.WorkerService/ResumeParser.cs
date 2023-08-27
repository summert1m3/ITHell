using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Application.Services.Parsers;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.Page;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ITHell.VacancyParser.WorkerService;

public class ResumeParser : BackgroundService
{
    private readonly ILogger<ResumeParser> _logger;
    private readonly IFlareSolverrHttpClient _flareSolverrHttpClient;
    private readonly IHtmlParser _htmlParser;

    private readonly string _regionalJobSiteLink;
    private readonly string _parserStartLink;

    public ResumeParser(
        ILogger<ResumeParser> logger,
        IFlareSolverrHttpClient flareSolverrHttpClient,
        IHtmlParser htmlParser,
        IConfiguration configuration)
    {
        _logger = logger;
        _flareSolverrHttpClient = flareSolverrHttpClient;
        _htmlParser = htmlParser;
        _logger = logger;
        _flareSolverrHttpClient = flareSolverrHttpClient;
        _htmlParser = htmlParser;

        _regionalJobSiteLink = configuration["RegionalJobSiteLink"];
        Guard.Against.NullOrWhiteSpace(_regionalJobSiteLink);

        _parserStartLink = configuration["ResumeInitialPage"];
        Guard.Against.NullOrWhiteSpace(_parserStartLink);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await StartParse(_parserStartLink);
        }
    }

    private async Task ParseFromFile()
    {
        var config = Configuration.Default.WithDefaultLoader();
        using var context = BrowsingContext.New(config);

        var resumePageHtml = await File.ReadAllTextAsync("");
            
        using var resumePageDoc
            = await context.OpenAsync(req => req.Content(resumePageHtml));
            
        await resumePageDoc.WaitForReadyAsync();
            
        var resumePage
            = _htmlParser.ParseResumePage(resumePageDoc, new ResumeCard()
            {
                EmployeeStatus = EmployeeStatus.Active,
                ResumeId = Guid.NewGuid(),
                ResumePageLink = ""
            });
    }

    private async Task StartParse(string pageLink)
    {
        var pageNumber = 0;

        List<ResumeCard> allResumeCards = new();
        List<ResumePage> allResumePages = new();
        
        var config = Configuration.Default.WithDefaultLoader();
        using var context = BrowsingContext.New(config);

        while (true)
        {
            Console.WriteLine($"PAGE {pageNumber}");

            var html = await _flareSolverrHttpClient.Get(pageLink);

            using var doc = await context.OpenAsync(req => req.Content(html));
            await doc.WaitForReadyAsync();

            var resumeCards = _htmlParser.ParseResumeCards(doc);

            allResumeCards.AddRange(resumeCards);

            foreach (var resumeCard in resumeCards)
            {
                DisplayResumeCard(resumeCard);

                await Task.Delay(new TimeSpan(0, 0, Random.Shared.Next(5, 15)));

                var resumePageHtml = await _flareSolverrHttpClient.Get(resumeCard.ResumePageLink);

                using var resumePageDoc
                    = await context.OpenAsync(req => req.Content(resumePageHtml));
                await doc.WaitForReadyAsync();

                var resumePage
                    = _htmlParser.ParseResumePage(resumePageDoc, resumeCard);
                
                allResumePages.Add(resumePage);

                DisplayResumePage(resumePage);
            }

            Console.WriteLine(resumeCards.Count);

            var nextLink = doc.QuerySelector("div[data-qa=\"pager-block\"] a[data-qa=\"pager-next\"]")?
                .GetAttribute("href");

            if (nextLink is null)
            {
                break;
            }

            pageLink = _regionalJobSiteLink + nextLink;

            await Task.Delay(new TimeSpan(0, 0, Random.Shared.Next(15, 30)));

            pageNumber++;
        }

        Console.WriteLine($"All resume cards: {allResumeCards.Count}");
        Console.WriteLine($"All resume pages: {allResumePages.Count}");
    }

    private static void DisplayResumePage(ResumePage resume)
    {
        Console.WriteLine();
        Console.WriteLine(resume);
        Console.WriteLine();
    }

    private static void DisplayResumeCard(ResumeCard resumeCard)
    {
        Console.WriteLine();
        Console.WriteLine(resumeCard);
        Console.WriteLine();
    }
}