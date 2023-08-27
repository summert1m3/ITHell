using FlareSolverrSharp;

namespace ITHell.VacancyParser.Application.Services.Clients;

public class FlareSolverrHttpClient : IFlareSolverrHttpClient
{
    private readonly HttpClient _httpClient;

    public FlareSolverrHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Get(string pageLink)
    {
        var content = await _httpClient.GetStringAsync(pageLink);

        return content;
    }
}