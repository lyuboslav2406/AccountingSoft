namespace AccountingSoft.Web.ViewModels.Product
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Client = AccountingSoft.Web.ViewModels.Client.ClientViewModel;

    public class AllProductsViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }

        public string ProductName { get; set; }

        public decimal Qty { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public Client Client { get; set; }

        public System.Guid ClientId { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }
    }
}
