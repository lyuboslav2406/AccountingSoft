namespace AccountingSoft.Data.Models
{
    using AccountingSoft.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
        }

        [Key]
        public Guid Id { get; set; }

        public string ProductName { get; set; }

        public decimal Qty { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public System.Guid ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
