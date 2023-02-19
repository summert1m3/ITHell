using FlareSolverrSharp;

namespace ITHell.VacancyParser.Application.Services.Clients;

public interface IFlareSolverrHttpClient
{
    public Task<string> Get(string pageLink);
}