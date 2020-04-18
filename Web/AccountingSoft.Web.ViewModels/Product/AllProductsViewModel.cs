using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingSoft.Web.ViewModels.Product
{
    public class AllProductsViewModel
    {
        public IEnumerable<ProductViewModel> Clients { get; set; }
    }
}
