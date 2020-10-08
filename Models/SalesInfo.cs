using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Models
{
    public class SalesInfo
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Cost { get; set; }

        public int SalesInvoiceId { get; set; }
    }
}
