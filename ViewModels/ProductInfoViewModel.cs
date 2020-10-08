using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.ViewModels
{
    public class ProductInfoViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public decimal Cost { get; set; }
    }
}
