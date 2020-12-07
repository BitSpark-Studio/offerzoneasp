using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfferZoneAsp.Models;
using OfferZoneAsp.Shared;

namespace OfferZoneAsp.Controllers
{
    public class OffersController : Controller
    {
        private readonly OfferContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public OffersController(OfferContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Create(OfferViewModel model)
        {
            if (ModelState.IsValid)
            {
                String UploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "OfferImages");
                String UniqueFileName = Guid.NewGuid().ToString() + "_" + model.OfferImageName.FileName;
                String FilePath = Path.Combine(UploadFolder, UniqueFileName);
                model.OfferImageName.CopyTo(new FileStream(FilePath, FileMode.Create));

                var offermodel = new Offer
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    ContactNumber = model.ContactNumber,
                    Location = model.Location,
                    OfferImageName = UniqueFileName,
                    CreatedAt = DateTime.Now,
                    ExpiredAt=model.ExpiredAt,
                    FbLink = model.FbLink,
                    InstagramLink=model.InstagramLink,
                    WebsiteLink=model.WebsiteLink,
                    CategoryId = model.CategoryId
                };
                _context.Add(offermodel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
