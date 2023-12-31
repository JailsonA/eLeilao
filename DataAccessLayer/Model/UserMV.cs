﻿using DataAccessLayer.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class UserMV
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserTypeEnum? UserType { get; set; }
        public int AuctionId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
