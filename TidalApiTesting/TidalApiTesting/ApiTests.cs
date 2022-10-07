using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibrary;
using System.Net;

namespace TidalApiTesting
{
    public class Tests
    {
        private const string GetStationsUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?";
        private const string GetHirtaStationUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/0322?";

        [Test]
        public async Task GetStationsReturnsSuccessful()
        {
            var settings = CreateSettingsConfig();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            var response = await client.GetAsync(GetStationsUri);

            response.Should().BeSuccessful();
        }

        [Test]
        public async Task GetStationsReturnsUnauthorised()
        {
            using var client = new HttpClient();

            var response = await client.GetAsync(GetStationsUri);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetStationReturnsSuccessful()
        {
            var settings = CreateSettingsConfig();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            var response = await client.GetAsync(GetHirtaStationUri); 

            response.Should().BeSuccessful();
        }

        [Test]
        public async Task GetStationReturnsStationName()
        {
            var settings = CreateSettingsConfig();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            var response = await client.GetAsync(GetHirtaStationUri);

            var content = await DeserializeContent(response);

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

        private async Task<Root> DeserializeContent(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var contentDeserialized = JsonConvert.DeserializeObject<Root>(content);
            return contentDeserialized;
        }
    }
}