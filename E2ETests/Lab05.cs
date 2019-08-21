using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreTest201908;
using AspNetCoreTest201908.Entity;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace E2ETests
{
    public class Lab05 : TestBase<Startup>
    {
        public Lab05(WebApplicationFactory<Startup> factory)
                : base(factory)
        {
        }

        [Fact]
        public async Task Test()
        {
            var httpClient = CreateHttpClient();

            var expected = new List<Profile>
            {
                new Profile { Id = Guid.NewGuid(), Name = "123" },
                new Profile { Id = Guid.NewGuid(), Name = "456" }
            };

            DbOperator(db =>
            {
                db.Profile.AddRange(expected);
                db.SaveChanges();
            });

            var response = await httpClient.GetAsync("/api/Lab05/Index1");

            response.EnsureSuccessStatusCode();

            var profiles = await response.Content.ReadAsAsync<List<Profile>>();

            profiles.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Test3()
        {
            var httpClient = CreateHttpClient();

            var expected = new List<Profile>
            {
                new Profile { Id = Guid.NewGuid(), Name = "123" },
                new Profile { Id = Guid.NewGuid(), Name = "456" }
            };

            DbOperator(db =>
            {
                db.Profile.AddRange(expected);
                db.SaveChanges();
            });

            var response = await httpClient.GetAsync("/api/Lab05/Index3");

            response.EnsureSuccessStatusCode();

            var profiles = await response.Content.ReadAsAsync<List<VProfile>>();

            profiles.Should().BeEquivalentTo(expected.Select(a => new VProfile { Name = a.Name }));
        }

        [Fact]
        public async Task Test2()
        {
            var httpClient = CreateHttpClient();
            var profile = new ProfileDto
            {
                Name = "ABC"
            };
            var response = await httpClient.PostAsJsonAsync("/api/Lab05/Index2", profile);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<Profile>();

            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be("ABC");

            DbOperator(db =>
            {
                var dbProfile = db.Profile.First();
                dbProfile.Id.Should().NotBeEmpty();
                dbProfile.Name.Should().Be("ABC");
            });
        }
    }
}