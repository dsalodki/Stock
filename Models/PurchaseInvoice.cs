using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Models
{
    public class PurchaseInvoice
    {
        public int Id { get; set; }
        [DisplayName("driver")]
        public int BroughtId { get; set; }
        public DateTime When { get; set; }
        [DisplayName("loader")]
        public int? ShippedId { get; set; }
        [DisplayName("store keeper")]
        public int? CheckedId { get; set; }
        public List<ProductInfo> ProductInfos { get; set; }
        public bool IsOnStock { get; set; }
    }
}
