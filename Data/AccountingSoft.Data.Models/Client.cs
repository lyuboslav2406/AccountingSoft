using AccountingSoft.Data.Common.Models;
using AccountingSoft.Services.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingSoft.Data.Models
{
    public class Client : BaseDeletableModel<int>
    {
        public Client()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EIK { get; set; }

        public bool DDS { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}
