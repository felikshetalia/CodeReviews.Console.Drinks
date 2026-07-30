using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace CodeReviews.Console.Drinks;

class Program
{
    static void Main(string[] args)
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

        IAppView appview = new AppView();
        IDrinksView drinksView = new DrinksView();

        DrinksController drinksController = new(drinksView);
        AppController mainApp = new(appview, drinksController);

        mainApp.Run();
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
