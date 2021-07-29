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

        public async Task AddSellingProduct(Product product, decimal sellingQty, DateTime crOn)
        {
            var soldProduct = new SoldProduct()
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                SoldQty = sellingQty,
            };
            soldProduct.CreatedOn = crOn;
            product.Qty = product.Qty - sellingQty;
            product.Sum = product.Sum - (product.SinglePrice * sellingQty);
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

        public async Task DeleteSoldProduct(SoldProduct id)
        {
            var soldProduct = this.soldProductRepository.Find(id.Id);
            var product = this.productRepository.Find(soldProduct.ProductId);
            product.Sum = product.Sum + (soldProduct.SoldQty * product.SinglePrice);
            product.Qty += soldProduct.SoldQty;
            await this.soldProductRepository.SaveChangesAsync();
            this.soldProductRepository.Delete(soldProduct);
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

        public IEnumerable<T> GetAllProducts<T>(Guid id, DateTime startDate, DateTime endDate, string search = null, int? take = null, int skip = 0, bool forPdf = false)
        {
            IQueryable<Product> products = null;

            if (search != null)
            {
                 products = this.productRepository
                 .All().Where(p => p.Qty > 0 && p.ProductName.Contains(search))
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);
            }
            else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                products = this.productRepository
                 .All().Where(p => p.Qty > 0 && p.CreatedOn > startDate && p.CreatedOn < endDate)
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);
            }
            else
            {
                products = this.productRepository
                 .All().Where(p => p.Qty > 0)
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);
            }

            if (take.HasValue)
            {
                products = products.Where(a => a.ClientId == id).Take(take.Value);
            }

            if (forPdf)
            {
                products = products.Where(a => a.ClientId == id);
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

        public IEnumerable<T> GetAllSoldProducts<T>(Guid id, DateTime startDate, DateTime endDate)
        {
            IQueryable<SoldProduct> products = null;

            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                products = this.soldProductRepository
                 .All().Where(a => a.ProductId == id && a.CreatedOn > startDate && a.CreatedOn < endDate).Include(a => a.Product)
                 .OrderByDescending(x => x.CreatedOn);
            }
            else
            {
                products = this.soldProductRepository
                                 .All().Where(a => a.ProductId == id).Include(a => a.Product)
                                 .OrderByDescending(x => x.CreatedOn);
            }
            return products.To<T>();
        }

        public IEnumerable<T> GetAllZeroProducts<T>(Guid id, DateTime startDate, DateTime endDate, string search = null, int? take = null, int skip = 0, bool forPdf = false)
        {
            IQueryable<Product> products = null;

            if (search != null)
            {
                products = this.productRepository
                .All().Where(p => p.Qty == 0 && p.ProductName.Contains(search) && p.CreatedOn.Year <= ( DateTime.Today.Year - 1 ))
                .OrderByDescending(x => x.CreatedOn)
                .Skip(skip);
            }
            else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
            {
                products = this.productRepository
                 .All().Where(p => p.Qty == 0 && p.CreatedOn > startDate && p.CreatedOn < endDate)
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);
            }
            else
            {
                products = this.productRepository
                 .All().Where(p => p.Qty == 0 && p.CreatedOn.Year >= (DateTime.Today.Year - 1))
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip);
            }

            if (take.HasValue)
            {
                products = products.Where(a => a.ClientId == id).Take(take.Value);
            }

            if (forPdf)
            {
                products = products.Where(a => a.ClientId == id);
            }

            return products.To<T>();
        }

        public IEnumerable<T> GetAllZeroProducts<T>(string search = null, int? take = null, int skip = 0)
        {
            IQueryable<Product> products = this.productRepository
                  .All()
                  .OrderByDescending(x => x.CreatedOn)
                  .Skip(skip);

            if (search != null)
            {
                products = products.Where(a => a.ProductName.Contains(search) && a.Qty == 0);
            }

            if (take.HasValue)
            {
                products = products.Where(a => a.Qty == 0).Take(take.Value);
            }

            return products.To<T>();
        }

        public int GetCount()
        {
            var count = this.productRepository.All().Count();
            return count;
        }

        public Product GetProductById(Guid id)
        {
            var product = this.productRepository.Find(id);
            return product;
        }

        public SoldProduct GetSoldProductById(Guid id)
        {
            var product = this.soldProductRepository
                                 .All().Where(a => a.Id == id).Include(a => a.Product).FirstOrDefault();
            return product;
        }
    }
}
