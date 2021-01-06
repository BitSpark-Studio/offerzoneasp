using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfferZoneAsp.Models;

namespace OfferZoneAsp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OfferContext _context;
        public HomeController(ILogger<HomeController> logger, OfferContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Offers = await _context.Offers.ToListAsync();
            mymodel.Categories = await _context.Categories.ToListAsync();
            mymodel.Ratings = await _context.Ratings .ToListAsync();
            mymodel.ApplicationUsers = await _context.ApplicationUsers.ToListAsync();
            mymodel.recentThreeOffers = _context.Offers.OrderByDescending(x => x.CreatedAt).Take(3);
            return View(mymodel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}