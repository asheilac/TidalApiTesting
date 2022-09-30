using System.Net;
using System.Web;
using FluentAssertions;

namespace TidalApiTesting
{
    public class Tests
    {
        [Test]
        public async Task GetStationsReturnsOK()
        {
            using var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");

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

    }
}