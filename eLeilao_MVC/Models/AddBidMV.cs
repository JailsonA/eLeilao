namespace eLeilao_MVC.Models
{
    public class AddBidMV
    {
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public float BidValue { get; set; }
    }
}
