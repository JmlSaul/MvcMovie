using System;
using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers
{
    public class GenericController<T> : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Content($"hello from Generic Ctroller {typeof(T).Name}");
        }

        public IActionResult Get()
        {
            return View(typeof(T));
        }
    }
}