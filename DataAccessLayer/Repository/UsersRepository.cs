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
                    if (userLog != null && userLog.UserType == (int)UserTypeEnum.Admin)
                    {
                        var user = new UserModel()
                        {
                            FullName = userMV.FullName,
                            Contact = userMV.Contact,
                            Email = userMV.Email,
                            Password = userMV.Password,
                            UserType = 1,
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
    }
}
