﻿using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IProductRepository
    {
        bool AddProduct(ProductMV product);
        List<ProductModel> GetAllProduct();
    }
}
