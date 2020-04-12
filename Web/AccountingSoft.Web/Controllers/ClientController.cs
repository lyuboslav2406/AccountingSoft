namespace AccountingSoft.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class ClientController : Controller
    {
        public ClientController()
        {
        }

        public IActionResult ById()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
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
