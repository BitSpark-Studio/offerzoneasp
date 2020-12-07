using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OfferZoneAsp.Shared
{
    public class OfferViewModel
    {
        
        public int OfferId { get; set; }
        [Required]
        [Display(Name = "Offer Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Offer Description")]
        public string Description { get; set; }
        [Display(Name = "Offer Price")]
        public string Price { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "Offer Location")]
        public string Location { get; set; }
        [Display(Name = "Offer Flyer/Image")]
        public IFormFile OfferImageName { get; set; }
        [Required]
        [Display(Name = "Offer Published At")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Offer Expiring At")]
        public DateTime ExpiredAt { get; set; }

        [Required]
        [Display(Name = "Offer Updated At")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [Display(Name = "Facebook Link")]

        public string FbLink { get; set; }

        [Required]
        [Display(Name = "Instagram Link")]
        public string InstagramLink { get; set; }
        [Required]
        [Display(Name = "Twitter Link")]
        public string TwitterLink { get; set; }
        [Required]
        [Display(Name = "Website Link")]
        public string WebsiteLink { get; set; }


        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }
     
        public string UserId { get; set; }
    }
}
