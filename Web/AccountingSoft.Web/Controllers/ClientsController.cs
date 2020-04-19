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

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;
        private IMemoryCache memoryCache;

        public ClientsController(IClientService clientService, IMemoryCache memoryCache)
        {
            this.clientService = clientService;
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

        public IActionResult Delete()
        {
            return this.View();
        }

        public IActionResult Edit()
        {
            return this.View();
        }
    }
}
