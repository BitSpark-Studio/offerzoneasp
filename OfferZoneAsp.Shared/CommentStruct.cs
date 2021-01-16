using System;
using System.Collections.Generic;
using System.Text;

namespace OfferZoneAsp.Shared
{
    public class CommentStruct
    {
        public int OfferId { get; set; }
        public int Ratings { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
    }
}
