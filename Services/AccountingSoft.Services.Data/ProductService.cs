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
            this.productRepository.Delete(product);
            await this.productRepository.SaveChangesAsync();
        }

        public async Task EditProduct(Product product)
        {
            this.productRepository.Update(product);
            await this.productRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllProducts<T>(Guid id)
        {
            var products = this.productRepository
                 .All().Where(p => p.ClientId == id)
                 .OrderByDescending(x => x.CreatedOn);

            return products.To<T>();
        }

        public IEnumerable<T> GetAllProducts<T>()
        {
            var products = this.productRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn);

            return products.To<T>();
        }

        public Product GetProductById(Guid id)
        {
            var product = this.productRepository.All().Where(x => x.Id == id);

            return product.FirstOrDefault();
        }
    }
}
