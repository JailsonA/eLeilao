using DataAccessLayer.Data;
using DataAccessLayer.Data.Enum;
using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly eLeilaoDbContext _context;

        public ProductRepository(eLeilaoDbContext context)
        {
            _context = context;
        }
        public bool AddProduct(ProductMV product)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == product.UserId);
                if (product == null && user == null)
                {
                    return false;
                }
                else
                {
                    if (user.UserType == (int)UserTypeEnum.Admin)
                    {
                        var newProduct = new ProductModel
                        {
                            ProductName = product.ProductName,
                            ImgUrl = product.ImgUrl,
                            UserId = product.UserId,
                            CreatedAt = DateTime.Now
                        };
                        _context.Products.Add(newProduct);
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
