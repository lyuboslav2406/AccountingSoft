namespace AccountingSoft.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AccountingSoft.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
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
