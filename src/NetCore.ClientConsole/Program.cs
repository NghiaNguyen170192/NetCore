using IdentityModel.Client;
using NetCore.Infrastructure.Crawler;
using NetCore.Infrastructure.Models.IMDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
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
            //// discover endpoints from metadata
            //var disco = await DiscoveryClient.GetAsync("http://localhost:5002");
            //if (disco.IsError)
            //{
            //    Console.WriteLine(disco.Error);
            //    return;
            //}

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("user", "Password123!", "api1");

            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //    return;
            //}

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
