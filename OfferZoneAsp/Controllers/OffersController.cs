using Microsoft.AspNetCore.Mvc;

namespace OfferZoneAsp.Controllers
{
    public class OffersController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}