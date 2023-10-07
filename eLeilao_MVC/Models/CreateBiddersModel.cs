namespace eLeilao_MVC.Models
{
    public class CreateBiddersModel
    {
        public List<UserMV> Users { get; set; }
        public AuctionMV Auction { get; set; }
    }
}
