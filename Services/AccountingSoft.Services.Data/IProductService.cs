using AccountingSoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product p);

        Task<Product> EditProduct(Product p);

        Task<bool> DeleteProduct(Product p);

        Task<List<Product>> ListProducts(Product p);
    }
}
