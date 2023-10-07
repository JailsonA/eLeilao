using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class AuctionMV
    {
        public int AuctionId { get; set; }
        public string AuctionName { get; set; }
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public string AuctionDescription { get; set; }
        public bool AuctionStatus { get; set; }
        public int ProductId { get; set; }
        public int CreatorUserId { get; set; }
        public string? Message { get; set; }
        public string Address { get; set; }
        public float InitialValue { get; set; }
        public float? FinalValue { get; set; }
        public float? BidValue { get; set; }
    }
}
