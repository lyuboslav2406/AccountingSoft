namespace AccountingSoft.Web.ViewModels.Product
{
    using System;
    using AccountingSoft.Data.Models;
    using System.Collections.Generic;
    using AccountingSoft.Services.Mapping;
    using AccountingSoft.Web.ViewModels.Client;

    using Client = AccountingSoft.Web.ViewModels.Client.ClientViewModel;
    using System.ComponentModel.DataAnnotations;

    public class ProductViewModel : IMapTo<Product>, IMapFrom<Product>
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string ProductName { get; set; }

        [Required]
        public decimal Qty { get; set; }

        public decimal SellingQty { get; set; }

        [Required]
        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime CreatedOnForSelling { get; set; }

        public Client Client { get; set; }

        public System.Guid ClientId { get; set; }

        public int InvoiceNumber { get; set; }

        public decimal SoldQty { get; set; }

        public decimal SoldSum 
        {
            get
            {
                return SoldQty * SinglePrice;
            }
        }
    }
}
