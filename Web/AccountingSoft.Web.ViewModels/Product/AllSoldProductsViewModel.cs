using System;
using System.Collections.Generic;
using System.Text;
using Product1 = AccountingSoft.Data.Models.Product;

namespace AccountingSoft.Web.ViewModels.Product
{
    public class AllSoldProductsViewModel
    {
        public IEnumerable<SoldProductViewModel> SoldProducts { get; set; }

        public Product1 Product { get; set; }

        public decimal SoldQty { get; set; }

        public decimal SoldSum { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalSoldQty { get; set; }

        public string ClientName { get; set; }
    }
}
