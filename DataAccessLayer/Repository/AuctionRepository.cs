using DataAccessLayer.Data;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly eLeilaoDbContext _context;

        public AuctionRepository(eLeilaoDbContext context)
        {
            _context = context;
        }
        public bool CreateAuction(AuctionMV auctionMV)
        {
            try
            {
                //verify if conditions are valid
                var user = _context.Users.FirstOrDefault(x => x.UserId == auctionMV.CreatorUserId && x.UserType == 0);
                bool isDateValid = auctionMV.AuctionStartDate >= DateTime.Now && auctionMV.AuctionStartDate < auctionMV.AuctionEndDate;
                var product = _context.Products.FirstOrDefault(x => x.ProductId == auctionMV.ProductId);
                if (user == null || !isDateValid || product == null)
                {
                    return false;
                }

                AuctionModel auction = new AuctionModel
                {
                    AuctionName = auctionMV.AuctionName,
                    AuctionStartDate = auctionMV.AuctionStartDate,
                    AuctionEndDate = auctionMV.AuctionEndDate,
                    AuctionDescription = auctionMV.AuctionDescription,
                    AuctionStatus = true,
                    ProductId = product.ProductId,
                    CreatorUserId = user.UserId,
                    Message = auctionMV.Message,
                    CreatedAt = DateTime.Now
                };

                _context.Auctions.Add(auction);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
