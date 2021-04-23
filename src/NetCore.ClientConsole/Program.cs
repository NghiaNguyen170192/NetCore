using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCore.ClientConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();           
        }

        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44398");
            if (disco.IsError)
            {
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "modulusgrant",
                ClientSecret = "jKGHySpqOJJzXKn9zFr5H09CPujNpVAVgZLP5CGSRq0=",
                Scope = "api1",
            });

            if (tokenResponse.IsError)
            {
                return;
            }

            //Console.WriteLine(tokenResponse.Json);
            //Console.WriteLine("\n\n");

            //// call api
            //var client = new HttpClient();
            //client.SetBearerToken(tokenResponse.AccessToken);

            //var response = await client.GetAsync("http://localhost:5001/api/values");
            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(response.StatusCode);
            //}
            //else
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(JArray.Parse(content));
            //}

            ////ImdbCrawler crawler = new ImdbCrawler();
            ////crawler.RunAsync();

            ////var genres = TitleBasicMapper._genres.ToList().OrderBy(x => x).ToList().Select(x => new Genre { Name = x });
            ////foreach (var genre in genres)
            ////{
            ////    string json = JsonConvert.SerializeObject(genre, Formatting.Indented);
            ////    var response2 = await client.PostAsync("http://localhost:5001/api/genres", new StringContent(json, Encoding.UTF8, "application/json"));
            ////    if (!response2.IsSuccessStatusCode)
            ////    {
            ////        Console.WriteLine(response2.StatusCode);
            ////    }
            ////    else
            ////    {
            ////        var content = await response2.Content.ReadAsStringAsync();
            ////        Console.WriteLine(JObject.Parse(content));
            ////    }
            ////}

            //Console.ReadLine();
        }
    }
}
