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
    using Microsoft.Extensions.Hosting;

    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly IClientService clientService;
        private readonly IViewRenderService viewRenderService;
        private readonly IPdfService htmlToPdfConverter;
        private readonly IHostingEnvironment environment;
        private IMemoryCache memoryCache;
        private const int ItemsPerPage = 5;

        public ProductsController(IProductService productService, IClientService clientService, 
            IMemoryCache memoryCache, IViewRenderService viewRenderService, IPdfService htmlToPdfConverter,
            IHostingEnvironment environment)
        {
            this.productService = productService;
            this.clientService = clientService;
            this.memoryCache = memoryCache;

            this.viewRenderService = viewRenderService;
            this.htmlToPdfConverter = htmlToPdfConverter;
            this.environment = environment;
        }

        public IActionResult Index(int page = 1, string search = null)
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);

            var products = new AllProductsViewModel();

            if (idString != null)
            {
                Guid id = new Guid(idString);
                products.Products = this.productService.GetAllProducts<ProductViewModel>(id, search, ItemsPerPage, (page - 1) * ItemsPerPage);
            }
            else
            {
                products.Products = this.productService.GetAllProducts<ProductViewModel>(search, ItemsPerPage, (page - 1) * ItemsPerPage);
            }

            var count = this.productService.GetCount();

            products.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);

            if (products.PagesCount == 0)
            {
                products.PagesCount = 1;
            }

            products.CurrentPage = page;

            return this.View(products);
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
            product.Id = Guid.NewGuid();
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.productService.AddProduct(product);
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
        public async Task<IActionResult> Edit(ProductViewModel pr)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(pr);

            if (!this.ModelState.IsValid)
            {
                return this.View(pr);
            }

            await this.productService.EditProduct(product);
            return this.RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var pr = this.productService.GetProductById(id);
            var clients = this.clientService.GetAllClients<ClientDropDownViewModel>();
            var product = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(pr);
            product.Clients = clients;
            return this.View(product);

        }

        [HttpGet]
        public IActionResult SellingProduct(Guid id)
        {
            var pr = this.productService.GetProductById(id);
            var product = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(pr);
            var clients = this.clientService.GetAllClients<ClientDropDownViewModel>();
            product.Clients = clients;
            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> SellingProduct(ProductViewModel pr)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(pr);

            if (pr.SellingQty > pr.Qty)
            {
                return this.View(pr);
            }
            else
            {
                await this.productService.AddSellingProduct(product, pr.SellingQty);
                return this.RedirectToAction("Index", "Products");
            }
        }

        public IActionResult AllSoldProducts(Guid id)
        {
            var allSold = new AllSoldProductsViewModel();
            allSold.SoldProducts = this.productService.GetAllSoldProducts<SoldProductViewModel>(id);


            this.memoryCache.Set("SelectedProductId", id.ToString());
            return this.View(allSold);
        }

        [HttpGet]
        public async Task<IActionResult> ConvertToPdf()
        {
            object id;
            id = this.memoryCache.Get("SelectedProductId");
            var allSold = new AllSoldProductsViewModel();
            allSold.SoldProducts = this.productService.GetAllSoldProducts<SoldProductViewModel>(Guid.Parse(id.ToString()));
            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Products/AllSoldProducts.cshtml", allSold);
            var fileContents = this.htmlToPdfConverter.Convert(this.environment.ContentRootPath, htmlData);
            return this.File(fileContents, "application/pdf");
        }
    }
}
