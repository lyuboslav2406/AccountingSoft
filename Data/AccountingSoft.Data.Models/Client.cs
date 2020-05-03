namespace AccountingSoft.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AccountingSoft.Data.Common.Models;
    using AccountingSoft.Services.Mapping;

    public class Client : BaseDeletableModel<int>
    {
        public Client()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(9)]
        [MinLength(9)]
        public string EIK { get; set; }

        public bool DDS { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
