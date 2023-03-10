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

    private async Task Test()
    {
        var config = Configuration.Default.WithDefaultLoader();
        using var context = BrowsingContext.New(config);

        var resumePageHtml = File.ReadAllText("");
            
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

    private async Task ParseResumeCards(string pageLink)
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

                await Task.Delay(new TimeSpan(0, 0, Random.Shared.Next(5, 10)));

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

    private static void DisplayResumePage(ResumePage resume)
    {
        Console.WriteLine($"{resume.ResumeId}");
        Console.WriteLine($"{resume.PageLink}");
        Console.WriteLine($"{resume.EmployeeStatus}");
        Console.WriteLine($"{resume.Gender}");
        
        Console.WriteLine($"{resume.Age}");
        Console.WriteLine($"{resume.BirthDate}");
        Console.WriteLine($"{resume.HasPicture}");
        Console.WriteLine($"{resume.Salary?.EstimatedSalary}");
        
        Console.WriteLine($"{resume.Salary?.SalaryCurrency}");
        Console.WriteLine($"{resume.Title}");

        Console.WriteLine();
        
        foreach (var employment in resume.Employments)
        {
            Console.WriteLine(employment);
        }

        Console.WriteLine();
        
        foreach (var workSchedule in resume.WorkSchedules)
        {
            Console.WriteLine(workSchedule);
        }

        Console.WriteLine();
        
        Console.WriteLine($"{resume.Experience}");
        Console.WriteLine($"{resume.Education}");
        
        Console.WriteLine($"{resume.Citizenship}");
        Console.WriteLine($"{resume.WorkPermit}");
    }

    private static void DisplayResumeCard(ResumeCard resumeCard)
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