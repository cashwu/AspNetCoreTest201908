using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreTest201908.Entity;
using AspNetCoreTest201908.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace E2ETests
{
    public class TestBase<TStartup> : IClassFixture<WebApplicationFactory<TStartup>>
            where TStartup : class
    {
        private readonly WebApplicationFactory<TStartup> _factory;
        protected WebApplicationFactory<TStartup> _webHost;

        public TestBase(WebApplicationFactory<TStartup> factory)
        {
            _factory = factory;
        }

        protected HttpClient CreateHttpClient(Action<IServiceCollection> configuration = null)
        {
            _webHost = _factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                
                if (configuration != null)
                {
                    builder.ConfigureTestServices(configuration);
                }

                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlite("DataSource=testdb");
                    });

                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopeServiceProvider = scope.ServiceProvider;
                        var appDbContext = scopeServiceProvider.GetRequiredService<AppDbContext>();

                        appDbContext.Database.EnsureDeleted();
                        appDbContext.Database.EnsureCreated();

                        appDbContext.Database.ExecuteSqlCommand(@"
                            create view V_Profile
                                as
                            select Name from Profile");
                    }
                });
            });

            return _webHost.CreateClient();
        }

        protected void DbOperator(Action<AppDbContext> action)
        {
            Operator(action);
        }
        
        protected void Operator<T>(Action<T> action)
        {
            using (var scope = _webHost.Server.Host.Services.CreateScope())
            {
                var t = scope.ServiceProvider.GetRequiredService<T>();
                action.Invoke(t);
            }
        }
    }
}