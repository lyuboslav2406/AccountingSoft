using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingSoft.Web.ViewModels.Product
{
    public class ProductViewModel
    {
        public string ProductName { get; set; }

        public decimal Qty { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }
    }
}
