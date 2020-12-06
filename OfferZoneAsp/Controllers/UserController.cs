using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OfferZoneAsp.Models;
using OfferZoneAsp.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OfferZoneAsp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly OfferContext context;
        readonly string vendor = "vendor";
        readonly string admin = "admin";
        readonly string gUser = "admin";


        public UserController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<ApplicationRole> _roleManager, OfferContext _context, IWebHostEnvironment _hostingEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            hostingEnvironment = _hostingEnvironment;
            context = _context;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }
        //POST: User/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    // Login is successful here, so we return now and the execution stops, meaning the bottom code never runs.
                    return RedirectToAction("index", "home");
                }
            }

            // If we get to this line, either the MoxelState isn't valid or the login failed.
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return RedirectToAction("Login", "User");
        }
        public IActionResult RegisterUser()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }
        public IActionResult RegisterVendor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterVendor(VendorRegistrationViewModel model, string ReturnUrl)
        {
            //TODO: Make user registration 
            if (ModelState.IsValid)
            {
                String UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "LogoImage");
                String UniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoImage.FileName;
                String FilePath = Path.Combine(UploadFolder, UniqueFileName);
                model.LogoImage.CopyTo(new FileStream(FilePath, FileMode.Create));

                // Copy data from RegisterViewModel to IdentityUser
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    Name = model.Fullname,
                    CompanyName = model.CompanyName,
                    UserType = vendor,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    Address = model.Address,
                    LogoImage = UniqueFileName
                };


                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Automatic signin 
                    await signInManager.SignInAsync(user, isPersistent: false);
                    // var imageStore = new UserImageStore
                    // {
                    //     UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    //     ImageName = UniqueFileName
                    // };
                    // context.Add(imageStore);
                    // await context.SaveChangesAsync();
                    //if the use has no role, give them student Role.
                    if (await roleManager.FindByNameAsync(vendor) == null)
                    {
                        await roleManager.CreateAsync(new ApplicationRole(vendor));
                    }
                    await userManager.AddToRoleAsync(user, vendor);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> RegisterUser(VendorRegistrationViewModel model, string ReturnUrl)
        {
            //TODO: Make user registration 
            if (ModelState.IsValid)
            {
                String UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "LogoImage");
                String UniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoImage.FileName;
                String FilePath = Path.Combine(UploadFolder, UniqueFileName);
                model.LogoImage.CopyTo(new FileStream(FilePath, FileMode.Create));

                // Copy data from RegisterViewModel to IdentityUser
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    Name = model.Fullname,
                    //CompanyName = model.CompanyName,
                    UserType = gUser,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    Address = model.Address,
                    LogoImage = UniqueFileName
                };


                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Automatic signin 
                    await signInManager.SignInAsync(user, isPersistent: false);
                    // var imageStore = new UserImageStore
                    // {
                    //     UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    //     ImageName = UniqueFileName
                    // };
                    // context.Add(imageStore);
                    // await context.SaveChangesAsync();
                    //if the use has no role, give them student Role.
                    if (await roleManager.FindByNameAsync(vendor) == null)
                    {
                        await roleManager.CreateAsync(new ApplicationRole(vendor));
                    }
                    await userManager.AddToRoleAsync(user, vendor);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        public IActionResult RegisterAdmin()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationViewModel model, string ReturnUrl)
        {
            //TODO: Make user registration 
            if (ModelState.IsValid)
            {
                String UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "LogoImage");
                String UniqueFileName = Guid.NewGuid().ToString() + "_" + model.LogoImage.FileName;
                String FilePath = Path.Combine(UploadFolder, UniqueFileName);
                model.LogoImage.CopyTo(new FileStream(FilePath, FileMode.Create));

                // Copy data from RegisterViewModel to IdentityUser
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    Name = model.Fullname,
                    //CompanyName = model.CompanyName,
                    UserType = admin,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    Address = model.Address,
                    LogoImage = UniqueFileName
                };


                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Automatic signin 
                    await signInManager.SignInAsync(user, isPersistent: false);
                    // var imageStore = new UserImageStore
                    // {
                    //     UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    //     ImageName = UniqueFileName
                    // };
                    // context.Add(imageStore);
                    // await context.SaveChangesAsync();
                    //if the use has no role, give them student Role.
                    if (await roleManager.FindByNameAsync(admin) == null)
                    {
                        await roleManager.CreateAsync(new ApplicationRole(admin));
                    }
                    await userManager.AddToRoleAsync(user, admin);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }


}