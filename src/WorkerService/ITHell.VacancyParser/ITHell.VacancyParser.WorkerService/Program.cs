using ITHell.VacancyParser.Application;
using ITHell.VacancyParser.Domain;
using ITHell.VacancyParser.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        
        services.AddDomain(configuration);
        services.AddApplication(configuration);
        
        services.AddHostedService<VacancyParser>();
        services.AddHostedService<ResumeParser>();
    })
    .Build();

host.Run();