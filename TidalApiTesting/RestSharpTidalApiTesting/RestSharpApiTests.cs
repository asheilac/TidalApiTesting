using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using RestSharp;
using SharedLibrary;

namespace RestSharpTidalApiTesting
{
    public class Tests
    {
        private const string GetStationsUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?";

        [Test]
        public void GetStationsReturnsSuccessful()
        {
            var settings = CreateSettingsConfig();

            var client = new RestClient(GetStationsUri);
            var request = new RestRequest();
            request.AddHeader("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");
            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private static Settings CreateSettingsConfig()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json")
                .AddEnvironmentVariables()
                .Build();

            Settings settings = config.GetRequiredSection("Settings").Get<Settings>();
            return settings;
        }

    }
}