using System.Configuration;
using System.Net;
using System.Web;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibrary;

namespace TidalApiTesting
{
    public class Tests
    {
        private const string getStationsUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?";
        private const string getHirtaStationUri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/0322?";

        [Test]
        public async Task GetStationsReturnsSuccessful()
        {
            var settings = CreateSettingsConfig();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            var response = await client.GetAsync(getStationsUri);

            response.Should().BeSuccessful();
        }


        [Test]
        public async Task GetStationsReturnsUnauthorised()
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");

            var response = await client.GetAsync(getStationsUri);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetStationReturnsSuccessful()
        {
            var settings = CreateSettingsConfig();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            var response = await client.GetAsync(getHirtaStationUri); 

            response.Should().BeSuccessful();
        }

        [Test]
        public async Task GetStationReturnsStationName()
        {
            var settings = CreateSettingsConfig();

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            var response = await client.GetAsync(getHirtaStationUri);

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