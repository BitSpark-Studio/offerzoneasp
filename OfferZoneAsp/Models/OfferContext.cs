using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OfferZoneAsp.Models
{
    public class OfferContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public OfferContext(DbContextOptions<OfferContext> options):base(options)
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}