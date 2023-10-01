using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data.Enum;
using DataAccessLayer.Utils;

namespace DataAccessLayer.Model
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserTypeEnum UserType { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; }


        // Propriedades de navegação para acessar os objetos relacionados
        public List<AssociationModel>? Associations { get; set; }
        public List<BidModel>? Bids { get; set; }

        /*encarregar o model com a responsabilidade de gerenciar as senhas*/
        public bool PasswordIsValid(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public void SetPasswordHash()
        {
            Password = Password.GerarHash();
        }

        public string SetNewPassword(string password)
        {
            string newPassword = Guid.NewGuid().ToString().Substring(0, 6);
            Password = newPassword.GerarHash();
            return newPassword;
        }
    }
}
