using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OfferZoneAsp.Models
{
    public class Offer
    {
        [Key]
        public int OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime UpdatedAt { get; set; }



        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Categories { get; set; }



        [ForeignKey("SocialLink")]
        public int SocialLinkId { get; set; }
        public SocialLink SocialLinks { get; set; }



        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
    }
}