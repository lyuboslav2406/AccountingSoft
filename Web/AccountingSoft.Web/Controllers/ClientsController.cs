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

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
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

        public IActionResult Index()
        {
            var list = this.clientService.GetAllClients<ClientViewModel>();

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
