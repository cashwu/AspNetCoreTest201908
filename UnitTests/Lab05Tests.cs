using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTest201908.Api.Lab05_DbContext;
using AspNetCoreTest201908.Entity;
using AspNetCoreTest201908.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class Lab05Tests
    {
        [Fact]
        public async Task Db01()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                          .UseInMemoryDatabase("name01")
                          .Options;

            AppDbContext appDbContext = new AppDbContext(options);

            var expected = new List<Profile>
            {
                new Profile
                {
                    Id = Guid.NewGuid(),
                    Name = "cash"
                }
            };
            appDbContext.Profile.AddRange(expected);
            appDbContext.SaveChanges();

            var lab05Controller = new Lab05Controller(appDbContext);

            var result = await lab05Controller.Index1();

            result.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Db02()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                          .UseInMemoryDatabase("name02")
                          .Options;

            AppDbContext appDbContext = new AppDbContext(options);
            var lab05Controller = new Lab05Controller(appDbContext);

            var profileDto = new ProfileDto
            {
                Name = "cash"
            };
            var result = (await lab05Controller.Index2(profileDto)) as OkObjectResult;

            var profile = result.Value.As<Profile>();
            profile.Id.Should().NotBeEmpty();
            profile.Name.Should().Be("cash");

            var dbProfile = appDbContext.Profile.First();
            dbProfile.Id.Should().NotBeEmpty();
            dbProfile.Name.Should().Be("cash");
        }

        [Fact]
        public async Task Db03()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                          .UseSqlite("DataSource=:memory:")
                          .Options;

            AppDbContext appDbContext = new AppDbContext(options);

            appDbContext.Database.OpenConnection();
            appDbContext.Database.EnsureCreated();

            var expected = new List<Profile>
            {
                new Profile
                {
                    Id = Guid.NewGuid(),
                    Name = "cash"
                }
            };
            appDbContext.Profile.AddRange(expected);
            appDbContext.SaveChanges();

            var lab05Controller = new Lab05Controller(appDbContext);

            var result = (await lab05Controller.Index1()).Value;

            result.Should().BeEquivalentTo(expected);

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.CloseConnection();
        }

        [Fact]
        public async Task Db04()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                          .UseSqlite("DataSource=:memory:")
                          .Options;

            AppDbContext appDbContext = new AppDbContext(options);

            appDbContext.Database.OpenConnection();
            appDbContext.Database.EnsureCreated();

            var lab05Controller = new Lab05Controller(appDbContext);

            var profileDto = new ProfileDto
            {
                Name = "cash"
            };
            var result = (await lab05Controller.Index2(profileDto)) as OkObjectResult;

            var profile = result.Value.As<Profile>();
            profile.Id.Should().NotBeEmpty();
            profile.Name.Should().Be("cash");

            var dbProfile = appDbContext.Profile.First();
            dbProfile.Id.Should().NotBeEmpty();
            dbProfile.Name.Should().Be("cash");

            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.CloseConnection();
        }
    }
}