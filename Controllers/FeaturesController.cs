using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MvcMovie.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly ApplicationPartManager _applicationPartManager;

        public FeaturesController(ApplicationPartManager applicationPartManager)
        {
            _applicationPartManager = applicationPartManager;
        }

        // GET
        public IActionResult Index()
        {
            var controllers = new ControllerFeature();
            _applicationPartManager.PopulateFeature(controllers);
            ViewBag.Controllers = controllers.Controllers.ToList();

            var metatada = new MetadataReferenceFeature();
            _applicationPartManager.PopulateFeature(metatada);
            ViewBag.MetaDatas = metatada.MetadataReferences.ToList();

            var taghelper = new TagHelperFeature();
            _applicationPartManager.PopulateFeature(taghelper);
            ViewBag.TagHelpers = taghelper.TagHelpers.ToList();

            var component = new ViewComponentFeature();
            _applicationPartManager.PopulateFeature(component);
            ViewBag.Components = component.ViewComponents.ToList();

            return View();
        }
    }
}