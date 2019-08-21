using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreTest201908;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace E2ETests
{
    public class Lab06 : TestBase<Startup>
    {
        public Lab06(WebApplicationFactory<Startup> factory)
                : base(factory)
        {
        }

        [Fact]
        public async Task Test_true()
        {
            var httpClient = CreateHttpClient(a => a.AddScoped<IHttpService, FakeSuccessHttpClient>());
            
            var response = await httpClient.GetAsync("/api/Lab06/Index1");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AuthResult>();

            result.IsAuth.Should().BeTrue();
        }
        
        [Fact]
        public async Task Test_false()
        {
            var httpClient = CreateHttpClient(a => a.AddScoped<IHttpService, FakeFailedHttpClient>());
            
            var response = await httpClient.GetAsync("/api/Lab06/Index1");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<AuthResult>();

            result.IsAuth.Should().BeFalse();
        }
        
        [Fact(Skip = "404")]
        public void HttpService()
        {
            CreateHttpClient();
            
            Operator<IHttpService>(async s =>
            {
                var result = await s.IsAuthAsync();
                result.Should().BeFalse();
            });
        }

        [Fact(Skip = "404")]
        public async Task HttpService_UnitTest()
        {
            var serviceProvider = new ServiceCollection()
                                       .AddHttpClient()
                                       .AddScoped<IHttpService, HttpService>()
                                       .BuildServiceProvider();

            var httpService = serviceProvider.GetRequiredService<IHttpService>();

            var result = await httpService.IsAuthAsync();
            result.Should().BeFalse();
        }
    }

    class FakeSuccessHttpClient : IHttpService
    {
        public Task<bool> IsAuthAsync()
        {
            return Task.FromResult(true);
        }
    }
    
    class FakeFailedHttpClient : IHttpService
    {
        public Task<bool> IsAuthAsync()
        {
            return Task.FromResult(false);
        }
    }
}