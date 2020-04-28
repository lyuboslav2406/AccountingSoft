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

        [Required]
        [MaxLength(9)]
        public string ProductName { get; set; }

        [Required]
        public decimal Qty { get; set; }

        [Required]
        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public System.Guid ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }
    }
}
