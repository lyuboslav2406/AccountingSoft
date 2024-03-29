﻿namespace AccountingSoft.Web.Controllers
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

        public IActionResult Index(DateTime startDate, DateTime endDate, int page = 1, string search = null)
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);

            object clientName;
            this.memoryCache.TryGetValue("ClientName", out clientName);

            var products = new AllProductsViewModel();

            if (idString != null || (startDate != DateTime.MinValue && endDate != DateTime.MinValue))
            {
                Guid id = new Guid(idString);
                products.Products = this.productService.GetAllProducts<ProductViewModel>(id, startDate, endDate, search, ItemsPerPage, (page - 1) * ItemsPerPage, false);
            }
            else
            {
                products.Products = this.productService.GetAllProducts<ProductViewModel>(search, ItemsPerPage, (page - 1) * ItemsPerPage);
            }

            if (clientName == null)
            {
                clientName = "Не е избран клиент";
            }

            products.ClientName = clientName.ToString();
            var count = this.productService.GetCount();

            products.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);

            if (products.PagesCount == 0)
            {
                products.PagesCount = 1;
            }

            products.CurrentPage = page;

            return this.View(products);
        }

        public IActionResult IndexZeroProducts(DateTime startDate, DateTime endDate, int page = 1, string search = null)
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);

            object clientName;
            this.memoryCache.TryGetValue("ClientName", out clientName);

            var products = new AllProductsViewModel();

            if (idString != null || (startDate != DateTime.MinValue && endDate != DateTime.MinValue))
            {
                Guid id = new Guid(idString);
                products.Products = this.productService.GetAllZeroProducts<ProductViewModel>(id, startDate, endDate, search, ItemsPerPage, (page - 1) * ItemsPerPage, false);
            }
            else
            {
                products.Products = this.productService.GetAllZeroProducts<ProductViewModel>(search, ItemsPerPage, (page - 1) * ItemsPerPage);
            }

            if (clientName == null)
            {
                clientName = "Не е избран клиент";
            }

            products.ClientName = clientName.ToString();
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
            var viewModel = new ProductViewModel
            {
            };
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductViewModel input)
        {
            var product = AutoMapperConfig.MapperInstance.Map<Product>(input);
            product.Id = Guid.NewGuid();
            product.CreatedOn = input.CreatedOn;
            product.SoldQty = product.Qty;
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);
            product.ClientId = Guid.Parse(idString);

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
            var product = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(pr);
            return this.View(product);

        }

        [HttpGet]
        public IActionResult SellingProduct(Guid id)
        {
            var pr = this.productService.GetProductById(id);
            var product = AutoMapperConfig.MapperInstance.Map<ProductViewModel>(pr);
            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> SellingProduct(ProductViewModel pr)
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);
            if (idString != null)
            {
                pr.ClientId = Guid.Parse(idString);
                var product = AutoMapperConfig.MapperInstance.Map<Product>(pr);

                if (pr.SellingQty > pr.Qty)
                {
                    return this.View(pr);
                }
                else
                {
                    await this.productService.AddSellingProduct(product, pr.SellingQty, pr.CreatedOnForSelling);
                    return this.RedirectToAction("Index", "Products");
                }
            }
            else
            {
                return this.RedirectToAction("Index", "Products");
            }
        }

        [HttpGet]
        public IActionResult DeleteSellingProduct(Guid id)
        {
            var pr = this.productService.GetSoldProductById(id);
            var product = AutoMapperConfig.MapperInstance.Map<SoldProductViewModel>(pr);
            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSellingProduct(SoldProductViewModel pr)
        {
            var product = AutoMapperConfig.MapperInstance.Map<SoldProduct>(pr);
            await this.productService.DeleteSoldProduct(product);
            return this.RedirectToAction("Index", "Products");
        }

        public IActionResult AllSoldProducts(Guid id, DateTime startDate, DateTime endDate)
        {
            try
            {
                var allSold = new AllSoldProductsViewModel();
                allSold.SoldProducts = this.productService.GetAllSoldProducts<SoldProductViewModel>(id, startDate, endDate);
                this.memoryCache.Set("SelectedProductId", id.ToString());
                allSold.InvoiceNumber = this.productService.GetProductById(id).InvoiceNumber;
                return this.View(allSold);
            }
            catch (Exception e)
            {
                return this.View("~/Views/Products/Index");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> ConvertToPdfAllSoldProducts(DateTime startDate, DateTime endDate)
        {
            object id;
            id = this.memoryCache.Get("SelectedProductId");

            object clientName;
            this.memoryCache.TryGetValue("ClientName", out clientName);

            var product = this.productService.GetProductById(Guid.Parse(id.ToString()));

            var allSold = new AllSoldProductsViewModel();
            allSold.SoldProducts = this.productService.GetAllSoldProducts<SoldProductViewModel>(Guid.Parse(id.ToString()), startDate, endDate);
            allSold.TotalSoldQty = allSold.SoldProducts.Sum(s => s.SoldQty);
            allSold.SoldSum = allSold.TotalSoldQty * product.SinglePrice;
            allSold.ClientName = clientName.ToString();
            allSold.InvoiceNumber = this.productService.GetProductById(Guid.Parse(id.ToString())).InvoiceNumber;
            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Products/AllSoldProductsForPdf.cshtml", allSold);

            var fileContents = this.htmlToPdfConverter.Convert("wwwroot/js/", htmlData);

            return this.File(fileContents, "application/pdf;charset=utf-8");
        }
        
        [HttpGet]
        public async Task<IActionResult> ConvertToPdfAllProducts(DateTime startDate, DateTime endDate)
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);

            object clientName;
            this.memoryCache.TryGetValue("ClientName", out clientName);

            var products = new AllProductsViewModel();

            Guid id = new Guid(idString);
            products.Products = this.productService.GetAllProducts<ProductViewModel>(id, startDate, endDate, null, null, 0, true);
            products.ClientName = clientName.ToString();
            foreach (var p in products.Products)
            {
                var sum = p.Qty * p.SinglePrice;
                products.TotalSum += sum;
            }
            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Products/IndexForPdf.cshtml", products);

            var fileContents = this.htmlToPdfConverter.Convert("wwwroot/js/", htmlData);

            return this.File(fileContents, "application/pdf;charset=utf-8");
        }
        [HttpGet]
        public async Task<IActionResult> ConvertToPdfAllZeroProducts(DateTime startDate, DateTime endDate)
        {
            string idString = string.Empty;
            this.memoryCache.TryGetValue("ClientSelected", out idString);

            object clientName;
            this.memoryCache.TryGetValue("ClientName", out clientName);

            var products = new AllProductsViewModel();

            Guid id = new Guid(idString);
            products.Products = this.productService.GetAllZeroProducts<ProductViewModel>(id, startDate, endDate, null, null, 0, true);
            products.ClientName = clientName.ToString();
            foreach (var p in products.Products)
            {
                var sum = p.Qty * p.SinglePrice;
                products.TotalSum += sum;
            }

            var htmlData = await this.viewRenderService.RenderToStringAsync("~/Views/Products/IndexForZeroPdf.cshtml", products);

            var fileContents = this.htmlToPdfConverter.Convert("wwwroot/js/", htmlData);

            return this.File(fileContents, "application/pdf;charset=utf-8");
        }
    }
}
