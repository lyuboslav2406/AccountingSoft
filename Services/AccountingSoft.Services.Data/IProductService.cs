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

        //Task<System.Guid> CreateAsync(string name, decimal qty, decimal singlePrice, System.Guid clientId);

        Task EditProduct(Product product);

        Task DeleteProduct(Product product);

        IEnumerable<T> GetAllProducts<T>(Guid id, string search = null, int? take = null, int skip = 0);

        IEnumerable<T> GetAllProducts<T>(string search = null, int? take = null, int skip = 0);

        Product GetProductById(Guid id);

        Task<bool> DeleteAllClientProducts(Client c);

        int GetCount();
    }
}
