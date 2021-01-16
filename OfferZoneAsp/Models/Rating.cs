using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OfferZoneAsp.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int Ratings { get; set; }
        public int OfferId { get; set; }
        public string UserId { get; set; }
    }
}