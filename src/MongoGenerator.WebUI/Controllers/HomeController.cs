using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoGenerator.Core;
using MongoGenerator.WebUI.Models;

namespace MongoGenerator.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIndexServices _indexService;

        public HomeController(IIndexServices indexService)
        {
            _indexService = indexService;
        }

        public IActionResult Index()
        {
            ViewBag.Script = _indexService.Generate().Replace(Environment.NewLine, "&#13;&#10");
            return View();
        }

    }
}
