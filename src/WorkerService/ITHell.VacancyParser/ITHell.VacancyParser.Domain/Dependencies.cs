using Ardalis.GuardClauses;
using ITHell.VacancyParser.Domain.Services.Vacancy.VacancyCardParser;
using ITHell.VacancyParser.Domain.Services.Vacancy.VacancyPageParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITHell.VacancyParser.Domain;

public static class Dependencies
{
    public static void AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        var jobSiteLink = configuration["JobSiteLink"];
        Guard.Against.NullOrWhiteSpace(jobSiteLink);
        
        services.AddSingleton<IVacancyCardParser>(
            provider => new VacancyCardParser(jobSiteLink));

        services.AddSingleton<IVacancyPageParser, VacancyPageParser>();
    }
}