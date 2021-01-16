using System;
using System.Collections.Generic;
using System.Text;
namespace OfferZoneAsp.Shared
{
    public class CommentRatingViewModel
    {
        public int OfferId { get; set; }
        public int Ratings{ get; set; }
        public string UserId { get; set;}
        public string CommentText { get; set; }
        public List<CommentStruct> ListOfComments { get; set; }
       
    }
}
