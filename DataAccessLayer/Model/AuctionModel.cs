using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class AuctionModel
    {
        [Key]
        public int AuctionId { get; set; }
        public string AuctionName { get; set; }
        public DateTime AuctionStartDate { get; set; }
        public DateTime AuctionEndDate { get; set; }
        public string AuctionDescription { get; set; }
        public bool AuctionStatus { get; set; }
        public int ProductId { get; set; }
        public int CreatorUserId { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public float? BidValue { get; set; }
        public int? WinnerUserId { get; set; }

        // Propriedades de navegação para acessar os objetos relacionados
        public ProductModel Product { get; set; }
        public ICollection<AssociationModel> Associations { get; set; }
        public ICollection<BidModel> Bids { get; set; }
    }
}
