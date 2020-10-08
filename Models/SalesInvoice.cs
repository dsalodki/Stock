using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Models
{
    public class SalesInvoice
    {
        public int Id { get; set; }
        [DisplayName("driver")]
        public int? BroughtId { get; set; }
        public DateTime When { get; set; }
        [DisplayName("loader")]
        public int? ShippedId { get; set; }
        [DisplayName("store keeper")]
        public int? CheckedId { get; set; }
        public bool IsOutOfStock { get; set; }
        public List<SalesInfo> ProductInfos { get; set; }
    }
}
