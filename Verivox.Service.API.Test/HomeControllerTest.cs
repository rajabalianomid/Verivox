using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Verivox.Service.API.Test
{
    public class HomeControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public HomeControllerTest()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task home_controller_test_should_return_string_content()
        {
            var response = await _client.GetAsync("api/Home");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal("API Is Up", content);
        }
    }
}
