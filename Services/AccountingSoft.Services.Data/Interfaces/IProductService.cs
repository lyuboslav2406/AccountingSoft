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

        Task AddSellingProduct(Product product, decimal sellingQty);

        Task EditProduct(Product product);

        Task DeleteProduct(Product product);

        Task DeleteSoldProduct(SoldProduct soldProduct);

        IEnumerable<T> GetAllProducts<T>(Guid id, DateTime startDate, DateTime endDate, string search = null, int? take = null, int skip = 0, bool forPdf = false);

        IEnumerable<T> GetAllProducts<T>(string search = null, int? take = null, int skip = 0);

        IEnumerable<T> GetAllSoldProducts<T>(Guid id, DateTime startDate, DateTime endDate);

        Product GetProductById(Guid id);

        SoldProduct GetSoldProductById(Guid id);

        Task<bool> DeleteAllClientProducts(Client c);

        int GetCount();
    }
}
