namespace AccountingSoft.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using AccountingSoft.Data.Models;

    public interface IProductService
    {
        Task AddProduct(Product product);

        Task EditProduct(Product product);

        Task DeleteProduct(Product product);

        IEnumerable<T> GetAllProducts<T>();
    }
}
