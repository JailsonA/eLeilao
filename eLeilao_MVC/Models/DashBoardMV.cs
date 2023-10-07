namespace eLeilao_MVC.Models
{
    public class DashboardMV
    {
        public UserMV Users { get; set; }
        public List<AuctionMV> Auctions { get; set; }
        public AuctionMV AuctionsByUser { get; set; }
        public List<BidMV> Bids { get; set; }
    }
}
