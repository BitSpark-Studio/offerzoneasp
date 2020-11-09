using Microsoft.AspNetCore.Mvc;

namespace OfferZoneAsp.Controllers
{
    public class UserController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}