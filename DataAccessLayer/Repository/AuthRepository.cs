using DataAccessLayer.Data;
using DataAccessLayer.Utils;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly eLeilaoDbContext _context;
        private readonly IGenTokenFilter _genTokenFilter;

        public AuthRepository(eLeilaoDbContext context, IGenTokenFilter genTokenFilter)
        {
            _context = context;
            _genTokenFilter = genTokenFilter;
        }

        /*Login*/
        public string logIn(LoginModel login)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.Email.ToLower() == login.Email.ToLower());

                if (user != null && user.PasswordIsValid(login.Password) && user.status == 1)
                {
                    return _genTokenFilter.GenerateToken(user);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
