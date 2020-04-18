using AccountingSoft.Data.Common.Repositories;
using AccountingSoft.Data.Models;
using AccountingSoft.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public class ProductService : IProductService
    {
        private readonly IDeletableEntityRepository<Product> productRepository;

        public ProductService(IDeletableEntityRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task AddProduct(Product product)
        {
            await this.productRepository.AddAsync(product);
            await this.productRepository.SaveChangesAsync();
        }

        public async Task<Guid> CreateAsync(string name, decimal qty, decimal singlePrice, System.Guid clientId)
        {
            var product = new Product
            {
                ProductName = name,
                Qty = qty,
                SinglePrice = singlePrice,
                Sum = qty * singlePrice,
                ClientId = clientId,
            };

            await this.productRepository.AddAsync(product);
            await this.productRepository.SaveChangesAsync();
            return product.Id;
        }

        public Task DeleteProduct(Product product)
        {
             this.productRepository.Delete(product);

             return null;
        }

        public async Task EditProduct(Product product)
        {
            this.productRepository.Update(product);
            await this.productRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllProducts<T>()
        {
            var products = this.productRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn);

            return products.To<T>();
        }
    }
}
