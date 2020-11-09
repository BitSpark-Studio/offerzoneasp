using Microsoft.AspNetCore.Mvc;

namespace OfferZoneAsp.Controllers
{
    public class CategoriesController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}