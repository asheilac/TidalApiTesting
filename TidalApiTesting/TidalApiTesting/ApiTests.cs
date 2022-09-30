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
        [Test]
        public async Task GetStationsReturnsSuccessful()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json")
                .AddEnvironmentVariables()
                .Build();

            Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

            using var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            queryString["name"] = "{string}";
            var uri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?" + queryString;

            var response = await client.GetAsync(uri);
            response.Should().BeSuccessful();
        }

        [Test]
        public async Task GetStationsReturnsUnauthorised()
        {
            using var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");

            queryString["name"] = "{string}";
            var uri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?" + queryString;

            var response = await client.GetAsync(uri);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetStationReturnsSuccessful()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json")
                .AddEnvironmentVariables()
                .Build();

            Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

            using var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            queryString["name"] = "{string}";
            var uri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/0322?" + queryString;

            var response = await client.GetAsync(uri); 
            response.Should().BeSuccessful();
        }

        [Test]
        public async Task GetStationReturnsStationName()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.development.json")
                .AddEnvironmentVariables()
                .Build();

            Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

            using var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{settings.ApiKey}");

            queryString["name"] = "{string}";
            var uri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations/0322?" + queryString;

            var response = await client.GetAsync(uri);
            var content = await DeserializeContent(response);
            content.properties.Name.Should().Be("Hirta (Bagh A' Bhaile)");
        }
        
        private async Task<Root> DeserializeContent(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var contentDeserialized = JsonConvert.DeserializeObject<Root>(content);
            return contentDeserialized;
        }
    }
}