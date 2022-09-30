using System.Web;

namespace TidalApiConsoleApp
{
    static class Program
    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{subscription key}");

            // Request parameters
            queryString["name"] = "{string}";
            var uri = "https://admiraltyapi.azure-api.net/uktidalapi/api/V1/Stations?" + queryString;

            var response = await client.GetAsync(uri);
        }
    }
}