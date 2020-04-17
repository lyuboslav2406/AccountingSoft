namespace AccountingSoft.Web.ViewModels.Product
{
    using System;

    using Client = AccountingSoft.Web.ViewModels.Client.ClientViewModel;

    public class ProductViewModel
    {
        public string ProductName { get; set; }

        public decimal Qty { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }

        public Client Client { get; set; }
    }
}
