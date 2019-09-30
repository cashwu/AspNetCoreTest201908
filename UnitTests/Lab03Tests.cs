using AspNetCoreTest201908.Api.Lab03_IHostEnvironment;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Xunit;

namespace UnitTests
{
    public class Lab03Tests
    {
        [Fact]
        public void Host1()
        {
            IWebHostEnvironment environment = new TestHostingEnvironment()
            {
                EnvironmentName = "Production"
            };

            var lab03Controller = new Lab03Controller(environment);

            var result = lab03Controller.Index1() as OkObjectResult;

            result.Value.As<EnvResult>().Env.Should().Be("Dev");
        }

        [Fact]
        public void Host2()
        {
            IWebHostEnvironment environment = new TestHostingEnvironment()
            {
                EnvironmentName = "Dev"
            };

            var lab03Controller = new Lab03Controller(environment);

            var result = lab03Controller.Index1() as OkObjectResult;

            result.Value.As<EnvResult>().Env.Should().Be("Prod");
        }
    }

    public class TestHostingEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }

        public string ContentRootPath { get; set; }

        public string EnvironmentName { get; set; }

        public IFileProvider WebRootFileProvider { get; set; }

        public string WebRootPath { get; set; }
    }
}