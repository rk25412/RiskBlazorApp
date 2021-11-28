using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Clear.Risk.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostApplicationLifetime lifetime;

        public HomeController(IHostApplicationLifetime lifetime)
        {
            this.lifetime = lifetime;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StopApp()
        {
            lifetime.StopApplication();
            return Ok();
        }
    }
}
