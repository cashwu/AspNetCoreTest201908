using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreTest201908;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace E2ETests
{
    public class Lab02 : TestBase<Startup>
    {
        public Lab02(WebApplicationFactory<Startup> factory)
                : base(factory)
        {
        }

        [Fact]
        public async Task EnvConfigTest()
        {
            var httpClient = CreateHttpClient();

            var response = await httpClient.GetAsync("/api/Lab022/Index1");

            response.EnsureSuccessStatusCode();

            var serverResult = await response.Content.ReadAsAsync<ServerResult>();
            serverResult.Host.Should().Be("10.10.1.1");
        }
    }
}