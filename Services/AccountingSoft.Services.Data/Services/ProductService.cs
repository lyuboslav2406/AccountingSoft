namespace AccountingSoft.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AccountingSoft.Data.Common.Repositories;
    using AccountingSoft.Data.Models;
    using AccountingSoft.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IDeletableEntityRepository<Client> clientRepository;
        private readonly IDeletableEntityRepository<SoldProduct> soldProductRepository;

        public ProductService(IDeletableEntityRepository<Product> productRepository, IDeletableEntityRepository<Client> clientRepository, IDeletableEntityRepository<SoldProduct> soldProductRepository)
        {
            this.productRepository = productRepository;
            this.clientRepository = clientRepository;
            this.soldProductRepository = soldProductRepository;
        }

        public async Task AddProduct(Product product)
        {
            var findClient = this.clientRepository.Find(product.ClientId);
            if (findClient.DDS)
            {
                product.SinglePrice *= 1.2M;
            }

            product.Sum = product.SinglePrice * product.Qty;

            await this.productRepository.AddAsync(product);
            await this.productRepository.SaveChangesAsync();
        }

        public async Task AddSellingProduct(Product product, decimal sellingQty)
        {
            var soldProduct = new SoldProduct()
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                SoldQty = sellingQty
            };
            product.Qty = product.Qty - sellingQty;
            await this.soldProductRepository.AddAsync(soldProduct);
            await this.soldProductRepository.SaveChangesAsync();
            this.productRepository.Update(product);
            await this.productRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAllClientProducts(Client c)
        {
            var listOfProducts = this.productRepository.All().Where(p => p.ClientId == c.Id).ToList();
            int deleted = 0;
            foreach (var pr in listOfProducts)
            {
                this.productRepository.Delete(pr);
                deleted++;
            }

            return true ? deleted == listOfProducts.Count() : false;
        }

        public async Task DeleteProduct(Product product)
        {
            var listSoldProducts = this.soldProductRepository.All().Where(p => p.ProductId == product.Id).ToList();
            foreach (var pr in listSoldProducts)
            {
                this.soldProductRepository.Delete(pr);
            }
            await this.soldProductRepository.SaveChangesAsync();
            this.productRepository.Delete(product);
            await this.productRepository.SaveChangesAsync();
        }

        public async Task EditProduct(Product product)
        {
            var findClient = this.clientRepository.Find(product.ClientId);
            if (findClient.DDS)
            {
                product.SinglePrice *= 1.2M;
            }

            product.Sum = product.SinglePrice * product.Qty;

            this.productRepository.Update(product);
            await this.productRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllProducts<T>(Guid id, string search = null, int? take = null, int skip = 0, bool forPdf = false)
        {
            IQueryable<Product> products = this.productRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);

            if (search != null)
            {
                products = products.Where(a => a.ProductName.Contains(search) && a.Qty > 0);
            }

            if (take.HasValue)
            {
                products = products.Where(a => a.ClientId == id && a.Qty > 0).Take(take.Value);
            }

            if(forPdf)
            {
                products = products.Where(a => a.ClientId == id && a.Qty > 0);
            }

            return products.To<T>();
        }

        public IEnumerable<T> GetAllProducts<T>(string search = null, int? take = null, int skip = 0)
        {
            IQueryable<Product> products = this.productRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);

            if (search != null)
            {
                products = products.Where(a => a.ProductName.Contains(search) && a.Qty > 0);
            }

            if (take.HasValue)
            {
                products = products.Where(a => a.Qty > 0).Take(take.Value);
            }

            return products.To<T>();
        }

        public IEnumerable<T> GetAllSoldProducts<T>(Guid id)
        {
            IQueryable<SoldProduct> products = this.soldProductRepository
                 .All().Where(a => a.ProductId == id).Include(a => a.Product)
                 .OrderByDescending(x => x.CreatedOn);

            return products.To<T>();
        }

        public int GetCount()
        {
            var count = this.productRepository.All().Count();
            return count;
        }

        public Product GetProductById(Guid id)
        {
            var product = this.productRepository.All().Where(x => x.Id == id).Include(m => m.Client).FirstOrDefault();
            return product;
        }
    }
}
