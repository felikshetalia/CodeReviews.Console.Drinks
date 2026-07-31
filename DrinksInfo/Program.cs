using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CodeReviews.Console.Drinks;

class Program
{
    static async Task Main(string[] args)
    {
        var config = Setup();

        string baseUrl =
            config["DrinksDB:BaseUrl"]
            ?? throw new InvalidOperationException(
                "DrinksDB:BaseUrl is missing.");

        string apiKey =
            config["DrinksDB:ApiKey"]
            ?? throw new InvalidOperationException(
                "DrinksDB:ApiKey is missing.");

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/{apiKey.Trim('/')}/"),
            Timeout = TimeSpan.FromSeconds(15)
        };

        IAppView appview = new AppView();
        IDrinksView drinksView = new DrinksView();

        IDrinksService drinksService = new DrinksService(httpClient);

        DrinksController drinksController = new(drinksView, drinksService);
        AppController mainApp = new(appview, drinksController);

        await mainApp.Run();
    }

    private static IConfigurationRoot Setup()
    {
        IConfigurationRoot config =
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    "Configuration/appsettings.json",
                    optional: false)
                .Build();

        return config;
    }
}
