using Ardalis.GuardClauses;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Application.Services.Parsers;
using ITHell.VacancyParser.Domain.Services.Vacancy.CardParser;
using ITHell.VacancyParser.Domain.Services.Vacancy.PageParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITHell.VacancyParser.Application;

public static class Dependencies
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var jobSiteLink = configuration["JobSiteLink"];
        Guard.Against.NullOrWhiteSpace(jobSiteLink);
        
        services.AddSingleton<IVacancyCardParser>(
            provider => new VacancyCardParser(jobSiteLink));

        services.AddSingleton<IVacancyPageParser, VacancyPageParser>();

        var flareSolverrUrl = configuration["FlareSolverrUrl"];
        Guard.Against.NullOrWhiteSpace(flareSolverrUrl);
        
        services.AddSingleton<IFlareSolverrHttpClient>(
            provider => new FlareSolverrHttpClient(flareSolverrUrl));
        
        services.AddSingleton<IHtmlParser, DefaultHtmlParser>();
    }
}