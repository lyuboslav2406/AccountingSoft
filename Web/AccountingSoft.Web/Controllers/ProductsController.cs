namespace AccountingSoft.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingSoft.Data.Models;
    using AccountingSoft.Services.Data;
    using AccountingSoft.Services.Mapping;
    using AccountingSoft.Web.ViewModels.Product;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IClientService clientService;
        private IMemoryCache memoryCache;

        public ProductsController(IProductService productService, IClientService clientService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.clientService = clientService;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);
            IEnumerable<ProductViewModel> products;

            if (idString != null)
            {
                Guid id = new Guid(idString);
                products = this.productService.GetAllProducts<ProductViewModel>(id);
            }
            else
            {
                products = this.productService.GetAllProducts<ProductViewModel>();
            }

            return this.View(products.ToList());
        }

        public IActionResult Create()
        {
            var clients = this.clientService.GetAllClients<ClientDropDownViewModel>();

            var viewModel = new ProductViewModel
            {
                Clients = clients,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductViewModel input)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(input);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var productId = await this.productService.CreateAsync(input.ProductName, input.Qty, input.SinglePrice, input.ClientId);
            return this.RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var pr = this.productService.GetProductById(id);
            var product = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(pr);
            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel p)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(p);
            await this.productService.DeleteProduct(product);

            return this.RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel pr)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(pr);
            this.productService.EditProduct(product);
            return this.RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var pr = this.productService.GetProductById(id);
            var product = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(pr);
            var clients = this.clientService.GetAllClients<ClientDropDownViewModel>();
            product.Clients = clients;
            return this.View(product);
        }
    }
}
