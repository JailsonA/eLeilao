using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IAuctionRepository
    {
        bool CreateAuction(AuctionMV auction);
        bool AddBid(BidMV bidMV);

        List<AuctionModel> GetAllAuction(int userId);
        List<BidModel> GetAllBids(int userId);
        bool AuctionStatus(int auctionId);
        bool UpdateAuction(AuctionMV auctionMV);
        bool DeleteAuction(int auctionId);
        bool AddAuctionMessage(int auctionId, string message);
        AuctionModel GetAuctionByUser(int userId);
        AuctionModel GetAuctionById(int auctionId);
    }
}
