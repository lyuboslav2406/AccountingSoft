namespace AccountingSoft.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AccountingSoft.Data.Models;
    using AccountingSoft.Services.Data;
    using AccountingSoft.Web.ViewModels.Client;
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
        public IActionResult Create(ClientViewModel clientViewModel)
        {
            this.clientService.AddClient(clientViewModel.ToClient(clientViewModel));

            return this.RedirectToAction("Index", "ClientsController");
        }

        public IActionResult Index()
        {
            var list = this.clientService.GetAllClients<Client>();

            return this.View(list);
        }

        public IActionResult Delete()
        {
            return this.View();
        }

        public IActionResult Update()
        {
            return this.View();
        }
    }
}
