using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfferZoneAsp.Models;
using OfferZoneAsp.Shared;

namespace OfferZoneAsp.Controllers
{
    public class OffersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        private readonly OfferContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public OffersController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<ApplicationRole> _roleManager,OfferContext context, IWebHostEnvironment hostingEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Offers.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(OfferViewModel model)
        {
            var currentUser = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

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
                    WebsiteLink=model.WebsiteLink
                    //CategoryId = model.CategoryId,
                    //UserId = currentUser.Id
                    
                };
                _context.Add(offermodel);
                await _context.SaveChangesAsync();               
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        // GET: Labs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.OfferId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title, Description, Price,Location,ExpiredAt,FbLink,InstagramLink,ContactNumber")] Offer offer)
        {
            if (id != offer.OfferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.OfferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }

        // GET: Offer/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .FirstOrDefaultAsync(m => m.OfferId == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }


        // GET: offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FirstOrDefaultAsync(m => m.OfferId == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
