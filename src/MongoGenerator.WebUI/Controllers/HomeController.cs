using System;
using Microsoft.AspNetCore.Mvc;
using MongoGenerator.Core;
using MongoGenerator.Core.Services;

namespace MongoGenerator.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGeneratorServices _generator;

        public HomeController(IGeneratorServices generator)
        {
            _generator = generator;
        }

        public IActionResult Index()
        {
            ViewBag.Script = _generator.GenerateIndexes().Replace(Environment.NewLine, "&#13;&#10");
            return View();
        }

    }
}
