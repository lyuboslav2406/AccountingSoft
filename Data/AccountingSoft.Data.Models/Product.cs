using System;

namespace AccountingSoft.Data.Models
{
    public class Product
    {
        public string ProductName { get; set; }

        public decimal Qty { get; set; }

        public decimal SinglePrice { get; set; }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }
    }
}
