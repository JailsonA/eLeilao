using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class BidMV
    {
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public float BidValue { get; set; }
    }
}
