namespace eLeilao_MVC.Models
{
    public class BidMV
    {
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public float BidValue { get; set; }
    }
}
