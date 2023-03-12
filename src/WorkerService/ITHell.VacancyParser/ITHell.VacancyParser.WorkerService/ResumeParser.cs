using AngleSharp;
using AngleSharp.Dom;
using Ardalis.GuardClauses;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Application.Services.Parsers;
using ITHell.VacancyParser.Domain.Entities.Resume;
using ITHell.VacancyParser.Domain.Entities.Resume.ResumePage;
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

            await ParseResumeCards(_parserStartLink);

            await Task.Delay(new TimeSpan(1, 0, 0, 0), stoppingToken);
        }
    }

    async Task ParseResumeCards(string pageLink)
    {
        var pageNumber = 2;

        List<ResumeCard> allResumeCards = new();

        while (true)
        {
            Console.WriteLine($"PAGE {pageNumber}");

            var html = await _flareSolverrHttpClient.Get(pageLink);

            var config = Configuration.Default.WithDefaultLoader();
            using var context = BrowsingContext.New(config);

            using var doc = await context.OpenAsync(req => req.Content(html));
            await doc.WaitForReadyAsync();

            var resumeCards = _htmlParser.ParseResumeCards(doc);

            allResumeCards.AddRange(resumeCards);

            foreach (var resumeCard in resumeCards)
            {
                DisplayResumeCard(resumeCard);

                await Task.Delay(new TimeSpan(0, 0, Random.Shared.Next(10, 20)));

                var resumePageHtml = await _flareSolverrHttpClient.Get(resumeCard.ResumePageLink);

                using var resumePageDoc
                    = await context.OpenAsync(req => req.Content(resumePageHtml));
                await doc.WaitForReadyAsync();

                var resumePage
                    = _htmlParser.ParseResumePage(resumePageDoc, resumeCard);

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

            var delay = Random.Shared.Next(15, 45);

            await Task.Delay(new TimeSpan(0, 0, delay));

            pageNumber++;
        }

        Console.WriteLine($"All resume cards: {allResumeCards.Count}");
    }

    void DisplayResumePage(ResumePage resume)
    {
        Console.WriteLine($"ResumeId: {resume.ResumeId}");
        Console.WriteLine($"PageLink: {resume.PageLink}");
        Console.WriteLine($"Title: {resume.Title}");
        Console.WriteLine($"Age: {resume.Age}");
    }

    private void DisplayResumeCard(ResumeCard resumeCard)
    {
        Console.WriteLine($"ResumeId: {resumeCard.ResumeId}");
        Console.WriteLine($"ResumePageLink: {resumeCard.ResumePageLink}");
        Console.WriteLine($"Label:");
        
        Console.WriteLine(resumeCard.EmployeeStatus);

        Console.WriteLine();
        Console.WriteLine("---------------");
        Console.WriteLine();
    }
}