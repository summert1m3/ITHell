using ITHell.VacancyParser.Application;
using ITHell.VacancyParser.Domain;
using ITHell.VacancyParser.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        
        services.AddHostedService<VacancyParser>();
        
        services.AddDomain(configuration);
        services.AddApplication(configuration);
    })
    .Build();

host.Run();