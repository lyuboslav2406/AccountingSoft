namespace AccountingSoft.Web.ViewModels.Product
{
    using System;
    using AccountingSoft.Data.Models;
    using System.Collections.Generic;
    using AccountingSoft.Services.Mapping;
    using AccountingSoft.Web.ViewModels.Client;

    using Client = AccountingSoft.Web.ViewModels.Client.ClientViewModel;

    public class ProductViewModel : IMapTo<Product>, IMapFrom<Product>
    {
        public string ProductName { get; set; }

        public decimal Qty { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }

        public Client Client { get; set; }

        public System.Guid ClientId { get; set; }

        public IEnumerable<ClientDropDownViewModel> Clients { get; set; }

    }
}
