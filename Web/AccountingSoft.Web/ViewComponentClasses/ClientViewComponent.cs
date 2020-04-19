using AccountingSoft.Services.Data;
using AccountingSoft.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingSoft.Web.ViewComponentClasses
{
    public class ClientViewComponent : ViewComponent
    {
        private readonly IClientService clientService;

        public ClientViewComponent(IClientService clientService)
        {
            this.clientService = clientService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = this.clientService.GetListOfClients();

            var model = new LayoutViewModel()
            {
                Clients = list,
            };

            return View(model);
        }
    }
}
