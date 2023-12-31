﻿using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IUsersRepository
    {
        bool AddUser(UserMV userMV, int userLogged);
        bool DeleteUser(int userId, int userLogged);
        List<UserModel> GetAllUsers(int userLogged);
        List<UserMV> GetAllUsersByAuction(int auctionId);
    }
}
