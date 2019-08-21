using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTest201908.Entity;
using AspNetCoreTest201908.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTest201908.Api.Lab05_DbContext
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class Lab05Controller : Controller
    {
        private readonly AppDbContext _dbContext;

        public Lab05Controller(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Profile>>> Index1()
        {
            var list = await _dbContext.Profile.ToListAsync();
            return list;
        }

        [HttpPost]
        public async Task<IActionResult> Index2([FromBody] ProfileDto profileDto)
        {
            var profile = new Profile
            {
                Id = Guid.NewGuid(),
                Name = profileDto.Name
            };
            _dbContext.Profile.Add(profile);

            await _dbContext.SaveChangesAsync();

            return Ok(profile);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<VProfile>>> Index3()
        {
            var list = await _dbContext.VProfile.ToListAsync();
            return Ok(list);
        }
    }
}