using FlareSolverrSharp;

namespace ITHell.VacancyParser.Application.Services.Clients;

public class FlareSolverrHttpClient : IFlareSolverrHttpClient
{
    private readonly string _flareSolverrUrl;

    public FlareSolverrHttpClient(string flareSolverrUrl)
    {
        _flareSolverrUrl = flareSolverrUrl;
    }

    public async Task<string> Get(string pageLink)
    {
        var handler = new ClearanceHandler(_flareSolverrUrl)
        {
            MaxTimeout = 60000
        };

        var client = new HttpClient(handler);
        var content = await client.GetStringAsync(pageLink);

        return content;
    }
}