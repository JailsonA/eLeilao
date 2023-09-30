using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class BidModel
    {
        [Key]
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public float BidValue { get; set; }
        public DateTime BidDate { get; set; }

        // Propriedades de navegação para acessar os objetos relacionados
        public UserModel User { get; set; }
        public AuctionModel Auction { get; set; }
    }
}
