using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using SharedLibrary;

namespace RestSharpTidalApiTesting
{
    public class Tests
    {
        private const string GetStationsUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?";
        private const string GetHirtaStationUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/0322?";

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

        [Test]
        public void GetStationsReturnsUnauthorised()
        {
            var client = new RestClient(GetStationsUri);
            var request = new RestRequest();
            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void GetStationReturnsSuccessful()
        {
            var settings = CreateSettingsConfig();
            var client = new RestClient(GetHirtaStationUri);
            var request = new RestRequest();
            request.AddHeader("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");
            var response = client.Execute(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void GetStationReturnsStationName()
        {
            var settings = CreateSettingsConfig();
            var client = new RestClient(GetHirtaStationUri);
            var request = new RestRequest();
            request.AddHeader("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");
            var response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<Root>(response.Content);
            content.properties.Name.Should().Be("Hirta (Bagh A' Bhaile)");
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