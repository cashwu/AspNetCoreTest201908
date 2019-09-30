using AspNetCoreTest201908.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreTest201908.Api.Lab03_IHostEnvironment
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class Lab03Controller : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public Lab03Controller(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index1()
        {
            if (_environment.IsProduction())
            {
                return Ok(new EnvResult { Env = "Dev" });
            }

            return Ok(new EnvResult { Env = "Prod" });
        }
    }
}