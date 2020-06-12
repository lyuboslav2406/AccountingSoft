using AccountingSoft.Data.Models;
using AccountingSoft.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using Product1 = AccountingSoft.Data.Models.Product;

namespace AccountingSoft.Web.ViewModels.Product
{
    public class SoldProductViewModel : IMapTo<SoldProduct>, IMapFrom<SoldProduct>
    {
        public Guid Id { get; set; }

        public Product1 Product { get; set; }

        public decimal SoldQty { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
