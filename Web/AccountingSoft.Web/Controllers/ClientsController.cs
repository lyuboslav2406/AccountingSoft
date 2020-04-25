﻿namespace AccountingSoft.Web.Controllers
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

        public ClientsController(IClientService clientService, IProductService productService, IMemoryCache memoryCache)
        {
            this.clientService = clientService;
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public bool SaveClientToMemoryCache(string selected)
        {
            this.memoryCache.Set("ClientSelected", selected);
            return true;
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

            var clientId = await this.clientService.CreateAsync(input.Name, input.EIK, input.DDS);
            return this.RedirectToAction("Index", "Clients");
        }

        public IActionResult Index(string search = null)
        {
            var list = this.clientService.GetAllClients<ClientViewModel>(search);

            return this.View(list.ToList());
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
