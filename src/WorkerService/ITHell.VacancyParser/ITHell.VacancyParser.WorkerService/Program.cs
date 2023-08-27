using Ardalis.GuardClauses;
using FlareSolverrSharp;
using ITHell.VacancyParser.Application;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Domain;
using ITHell.VacancyParser.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddDomain(configuration);
        services.AddApplication(configuration);

        //services.AddHostedService<VacancyParser>();
        services.AddHostedService<ResumeParser>();

        var flareSolverrUrl = configuration["FlareSolverrUrl"];
        Guard.Against.NullOrWhiteSpace(flareSolverrUrl);

        services.AddHttpClient<IFlareSolverrHttpClient, FlareSolverrHttpClient>().ConfigurePrimaryHttpMessageHandler(
            () => new ClearanceHandler(flareSolverrUrl)
            {
                MaxTimeout = 60000,
            });
    })
    .Build();

host.Run();