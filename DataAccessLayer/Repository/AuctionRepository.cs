﻿using DataAccessLayer.Data;
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
                    CreatedAt = DateTime.Now,
                    InitialValue = auctionMV.InitialValue,
                    BidValue = auctionMV.BidValue
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

        public List<AuctionModel> GetAllAuction(int userId)
        {
            try
            {
                var auctions = _context.Auctions.Where(x => x.CreatorUserId == userId).ToList();
                //update auction status if time end
                foreach (var auction in auctions)
                {
                    if (auction.AuctionEndDate < DateTime.Now)
                    {
                        auction.AuctionStatus = false;
                        _context.SaveChanges();
                    }
                }

                return auctions;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //update auction status if time end  ou if admin finish auction
        public bool AuctionStatus(int auctionId)
        {
            //transaction
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var auction = _context.Auctions.FirstOrDefault(x => x.AuctionId == auctionId);
                if (auction != null)
                {
                    auction.AuctionStatus = !auction.AuctionStatus;
                    _context.SaveChanges();

                    var associations = _context.Associations.Where(x => x.AuctionId == auctionId).ToList();
                    foreach (var association in associations)
                    {
                        var user = _context.Users.FirstOrDefault(x => x.UserId == association.UserId);
                        if (user != null)
                        {
                            user.status = 0;
                            _context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                return true;
            }
        }

        //update auction info
        public bool UpdateAuction(AuctionMV auctionMV)
        {
            try
            {
                var auction = _context.Auctions.FirstOrDefault(x => x.AuctionId == auctionMV.AuctionId);
                if (auction != null)
                {
                    auction.AuctionName = auctionMV.AuctionName;
                    auction.AuctionStartDate = auctionMV.AuctionStartDate;
                    auction.AuctionEndDate = auctionMV.AuctionEndDate;
                    auction.AuctionDescription = auctionMV.AuctionDescription;
                    auction.Message = auctionMV.Message;
                    auction.InitialValue = auctionMV.InitialValue;
                    auction.BidValue = auctionMV.BidValue;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        //delete auction by update status
        public bool DeleteAuction(int auctionId)
        {
            try
            {
                var auction = _context.Auctions.FirstOrDefault(x => x.AuctionId == auctionId);
                if (auction != null)
                {
                    auction.AuctionStatus = false;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }

        //add auction message
        public bool AddAuctionMessage(int auctionId, string message)
        {
            try
            {
                var auction = _context.Auctions.FirstOrDefault(x => x.AuctionId == auctionId);
                if (auction != null)
                {
                    auction.Message = message;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }


        public bool AddBid(BidMV bidMV)
        {
            //transaction
            var transaction = _context.Database.BeginTransaction();
            try
            {
                //verify conditions
                var userBids = _context.Bids.Where(x => x.UserId == bidMV.UserId && x.AuctionId == bidMV.AuctionId).ToList();
                var user = _context.Users.FirstOrDefault(x => x.UserId == bidMV.UserId);
                var auction = _context.Auctions.FirstOrDefault(x => x.AuctionId == bidMV.AuctionId);
                if (user == null || auction == null || userBids.Count >= 3)
                {
                    return false;
                }
                else
                {
                    if (bidMV.BidValue == auction.BidValue)
                    {
                        var bid = new BidModel
                        {
                            UserId = bidMV.UserId,
                            AuctionId = bidMV.AuctionId,
                            BidValue = bidMV.BidValue,
                            BidDate = DateTime.Now
                        };
                        _context.Bids.Add(bid);
                        _context.SaveChanges();

                        auction.FinalValue += bidMV.BidValue;
                        auction.WinnerUserId = bidMV.UserId;
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public List<BidModel> GetAllBids(int userId)
        {
            try
            {
                var bids = _context.Bids.Where(x => x.UserId == userId).ToList();
                return bids;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
