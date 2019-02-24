using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using System.Threading.Tasks;

namespace NetCore.UnitTests
{
    public class ConnectivityTest
    {
        private readonly string API_URL;
        private readonly string AUTHSERVER_URL;
        private readonly TestServer _authServer;
        private readonly TestServer _apiServer;
        public ConnectivityTest()
        {
            API_URL = "http://localhost:5001";
            AUTHSERVER_URL = "http://localhost:5002";
            _authServer = new TestServer(new WebHostBuilder().UseStartup<AuthServer.Startup>());
            _apiServer = new TestServer(new WebHostBuilder().UseStartup<API.Startup>());
        }

        [Fact]
        public async Task TestAuthServer()
        {
            using (var httpClient = _authServer.CreateClient())
            {
                var result = await httpClient.GetAsync(API_URL);
                Assert.True(result.IsSuccessStatusCode);
            }
        }
    }
}
