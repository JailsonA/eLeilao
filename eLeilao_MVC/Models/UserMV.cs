namespace eLeilao_MVC.Models
{
    public class UserMV
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string TokenKey { get; set; }
        public string? Contact { get; set; }
        public int? AuctionId { get; set; }
    }
}
