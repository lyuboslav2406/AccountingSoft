namespace AccountingSoft.Data.Models
{
    using AccountingSoft.Data.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    public class SoldProduct : BaseDeletableModel<int>
    {
        public Guid Id { get; set; }

        public decimal SoldQty { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}