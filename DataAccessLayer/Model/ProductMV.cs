using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class ProductMV
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ImgUrl { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
