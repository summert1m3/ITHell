using Ardalis.GuardClauses;
using ITHell.VacancyParser.Application.Services.Clients;
using ITHell.VacancyParser.Domain.Services.VacancyCardParser;
using ITHell.VacancyParser.Domain.Services.VacancyPageParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITHell.VacancyParser.Application;

public static class Dependencies
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var jobSiteLink = configuration["JobSiteLink"];
        Guard.Against.NullOrWhiteSpace(jobSiteLink);
        
        services.AddScoped<IVacancyCardParser>(
            provider => new VacancyCardParser(jobSiteLink));

        services.AddScoped<IVacancyPageParser, VacancyPageParser>();

        var flareSolverrUrl = configuration["FlareSolverrUrl"];
        Guard.Against.NullOrWhiteSpace(flareSolverrUrl);
        
        services.AddScoped<IFlareSolverrHttpClient>(
            provider => new FlareSolverrHttpClient(flareSolverrUrl));
    }
}