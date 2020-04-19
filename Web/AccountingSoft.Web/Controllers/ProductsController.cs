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

    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IClientService clientService;

        public ProductsController(IProductService productService, IClientService clientService)
        {
            this.productService = productService;
            this.clientService = clientService;
        }

        public IActionResult Index()
        {
            var products = this.productService.GetAllProducts<ProductViewModel>();

            return this.View(products.ToList());
        }

        public IActionResult ByClient()
        {
            return this.View();
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

        [HttpDelete]
        public IActionResult Delete(System.Guid productId)
        {
            return this.View();
        }

        public IActionResult Update()
        {
            return this.View();
        }
    }
}
