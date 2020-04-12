using AccountingSoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public class ProductService : IProductService
    {
        public Task<Product> AddProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public Task<Product> EditProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> ListProducts(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
