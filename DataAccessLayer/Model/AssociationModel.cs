using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class AssociationModel
    {
        [Key]
        public int AssociationId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }


        // Propriedades de navegação para acessar os objetos relacionados
        public UserModel User { get; set; }
        public AuctionModel Auction { get; set; }
    }

}
