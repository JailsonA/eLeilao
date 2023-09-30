using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int UserType { get; set; }
        public int status { get; set; }
        public DateTime CreatedAt { get; set; }


        // Propriedades de navegação para acessar os objetos relacionados
        public List<AssociationModel>? Associations { get; set; }
        public List<BidModel>? Bids { get; set; }
    }
}
