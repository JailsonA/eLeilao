using DataAccessLayer.Data;
using DataAccessLayer.Data.Enum;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly eLeilaoDbContext _context;

        public UsersRepository(eLeilaoDbContext context)
        {
            _context = context;
        }
        public bool AddUser(UserMV userMV, int userLogged)
        {
            //begin transaction
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (userMV == null)
                    return false;
                else
                {
                    var userLog = _context.Users.FirstOrDefault(x => x.UserId == userLogged);
                    if (userLog != null && userLog.UserType == UserTypeEnum.Admin)
                    {
                        var user = new UserModel()
                        {
                            FullName = userMV.FullName,
                            Contact = userMV.Contact,
                            Email = userMV.Email,
                            Password = userMV.Password,
                            UserType = UserTypeEnum.User,
                            status = 1
                        };
                        _context.Users.Add(user);
                        _context.SaveChanges();


                        if (userMV.AuctionId.HasValue)
                        {
                            var associat = new AssociationModel()
                            {
                                UserId = user.UserId,
                                AuctionId = userMV.AuctionId.Value
                            };
                            _context.Associations.Add(associat);
                            _context.SaveChanges();
                        }
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

        public bool DeleteUser(int userId, int userLogged)
        {
            //begin transaction
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var userLog = _context.Users.FirstOrDefault(x => x.UserId == userLogged);
                if (userLog != null && userLog.UserType == (int)UserTypeEnum.Admin)
                {
                    var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
                    if (user != null)
                    {
                        user.status = 0;
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public List<UserModel> GetAllUsers(int userLogged)
        {
            try
            {
                var userLog = _context.Users.FirstOrDefault(x => x.UserId == userLogged);
                if (userLog != null && userLog.UserType == (int)UserTypeEnum.Admin)
                {
                    var users = _context.Users.Where(x => x.status == 1).ToList();
                    if (users.Count > 0)
                    {
                        return users;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //return a list of all users by auction Associated
        public List<UserMV> GetAllUsersByAuction(int auctionId)
        {
            try
            {
                var userIds = _context.Associations
                    .Where(x => x.AuctionId == auctionId)
                    .Select(x => x.UserId)
                    .ToList();

                var usersList = _context.Users
                    .Where(x => userIds.Contains(x.UserId))
                    .Select(user => new UserMV
                    {
                        UserId = user.UserId,
                        FullName = user.FullName,
                        Contact = user.Contact,
                        Email = user.Email,
                    })
                    .ToList();

                return usersList;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
