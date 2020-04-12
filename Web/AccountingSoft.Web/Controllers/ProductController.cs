namespace AccountingSoft.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class ProductController : Controller
    {
        public IActionResult All()
        {
            return this.View();
        }

        public IActionResult ByClient()
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
