namespace AccountingSoft.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingSoft.Data.Models;
    using AccountingSoft.Services.Data;
    using AccountingSoft.Services.Mapping;
    using AccountingSoft.Web.ViewModels.Client;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class ClientsController : BaseController
    {
        private readonly IClientService clientService;
        private readonly IProductService productService;
        private IMemoryCache memoryCache;
        private const int ItemsPerPage = 5;

        public ClientsController(IClientService clientService, IProductService productService, IMemoryCache memoryCache)
        {
            this.clientService = clientService;
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public bool SaveClientToMemoryCache(string selected)
        {
            var clientGuid = Guid.Parse(selected);
            var client = this.clientService.GetSignleClient(clientGuid);
            this.memoryCache.Set("ClientName", client.Result.Name);
            this.memoryCache.Set("ClientSelected", selected);
            return true;
        }

        public IActionResult GetClientName()
        {
            object clientName;

            clientName = this.memoryCache.Get("ClientName");
            if (clientName == null)
            {
                clientName = string.Empty;
            }
            return this.Json(clientName.ToString());
        }

        public IActionResult ById()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ClientViewModel input)
        {
            var client = AutoMapperConfig.MapperInstance.Map<Client>(input);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.clientService.AddClient(client);
            return this.RedirectToAction("Index", "Clients");
        }

        public IActionResult Index(int page = 1, string search = null)
        {
            var viewModel = new AllClientViewModel();

            viewModel.Clients = this.clientService.GetAllClients<ClientViewModel>(search, ItemsPerPage, (page - 1) * ItemsPerPage);

            var count = this.clientService.GetCount();

            viewModel.PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var cl = this.clientService.GetSignleClient(id);
            var client = AutoMapperConfig.MapperInstance.Map<ClientViewModel>(cl.Result);
            return this.View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ClientViewModel c)
        {
            var client = AutoMapperConfig.MapperInstance.Map<Client>(c);
            var deleteProducts = await this.productService.DeleteAllClientProducts(client);
            if (deleteProducts)
            {
                await this.clientService.DeleteClient(client);
                return this.RedirectToAction("Index", "Clients");
            }
            else
            {
                return this.View();
            }
        }

        [HttpPost]
        public IActionResult Edit(ClientViewModel cl)
        {
            var client = AutoMapperConfig.MapperInstance.Map<Client>(cl);
            this.clientService.EditClient(client);
            return this.RedirectToAction("Index", "Clients");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var cl = this.clientService.GetSignleClient(id);
            var client = AutoMapperConfig.MapperInstance.Map<ClientViewModel>(cl.Result);
            return this.View(client);
        }
    }
}
