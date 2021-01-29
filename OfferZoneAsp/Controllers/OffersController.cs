using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewData["Categories"] = await _context.Categories.ToListAsync();
            ViewData["OfferTypes"] = await _context.OfferTypes.ToListAsync();

            if (currentUser != null)
            {
                if (currentUser.UserRole == "vendor" || currentUser.UserRole == "admin")
                {
                    return View();
                }
            }
            return Redirect("https://localhost:5001/unauthorized");

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
                    DiscountedPrice = model.DiscountedPrice,
                    ContactNumber = model.ContactNumber,
                    Location = model.Location,
                    OfferImageName = UniqueFileName,
                    CreatedAt = DateTime.Now,
                    ExpiredAt=model.ExpiredAt,
                    FbLink = model.FbLink,
                    InstagramLink=model.InstagramLink,
                    WebsiteLink=model.WebsiteLink,
                    CategoryId = model.CategoryId,
                    UserId = currentUser.Id,
                    OfferTypeId = model.OfferTypeId
                    
                    
                };
                _context.Add(offermodel);
                await _context.SaveChangesAsync();               
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
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
        public async Task<IActionResult> Edit(int id, [Bind("Title, Description, Price,DiscountedPrice,Location,ExpiredAt,FbLink,InstagramLink,ContactNumber")] Offer model)
        {
            var data = _context.Offers.Where(x => x.OfferId == id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    data.Title = model.Title;
                    data.Description = model.Description;
                    data.Price = model.Price;
                    data.DiscountedPrice = model.DiscountedPrice;
                    data.Location = model.Location;
                    data.ExpiredAt = model.ExpiredAt;
                    data.FbLink = model.FbLink;
                    data.InstagramLink = model.InstagramLink;
                    data.ContactNumber = model.ContactNumber;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(model.OfferId))
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
            return View(model);
        }
        public async Task<IActionResult> MyOffers(string id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            ViewData["OfferOwner"] = await _context.Offers.Where(x => x.UserId == id).ToListAsync(); 

            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            CommentRatingViewModel CRVM = new CommentRatingViewModel();
            CRVM.ListOfComments = new List<CommentStruct>();
            var offer = await _context.Offers.FirstOrDefaultAsync(m => m.OfferId == id);
            ViewData["CategoryInDetail"] = _context.Categories.Where(x => x.CategoryId == offer.CategoryId).FirstOrDefault();
            ViewData["OfferTypesInDetail"] = _context.OfferTypes.Where(x => x.OfferTypeId == offer.OfferTypeId).FirstOrDefault();
            ViewData["CurrentOffer"] = _context.Offers.Where(m => m.OfferId == id).FirstOrDefault();
            ViewData["OfferOwner"] = _context.ApplicationUsers.Where(x => x.Id == offer.UserId).FirstOrDefault();
            var UserCommentData = await _context.Comments.Where(m => m.OfferId == offer.OfferId).ToListAsync();

            foreach (var item in UserCommentData)
            {
                var ABC = new CommentStruct
                {
                    UserId = item.UserId,
                    OfferId = item.OfferId,
                    Comment = item.CommentText
                };
                CRVM.ListOfComments.Add(ABC);
            }

            //dynamic mymodel = new ExpandoObject();
            //mymodel.CategoryInDetail = _context.Categories.Where(x => x.CategoryId == offer.CategoryId).FirstOrDefault();
            //mymodel.OfferTypesInDetail = _context.Categories.Where(x => x.CategoryId == offer.OfferTypeId).FirstOrDefault();
            //mymodel.offer = await _context.Offers.FirstOrDefaultAsync(m => m.OfferId == id);
            //mymodel.OfferOwner = _context.ApplicationUsers.Where(x => x.Id == offer.UserId).FirstOrDefault();
            if (offer == null)
            {
                return NotFound();
            }

            return View(CRVM);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(CommentRatingViewModel model)
        {
            var currentUser = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    CommentText = model.CommentText,
                    UserId = currentUser.Id,
                    OfferId = model.OfferId
                };
                _context.Add(comment);
                await _context.SaveChangesAsync();

                var rating = new Rating
                {
                    Ratings = model.Ratings,
                    OfferId = model.OfferId,
                    UserId = currentUser.Id
                };
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return Redirect("https://localhost:5001/Offers/Details/"+model.OfferId);
            }
            return View(model);
        }

    }
}
