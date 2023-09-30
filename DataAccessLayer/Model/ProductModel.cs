using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ImgUrl { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
