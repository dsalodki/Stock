using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Models
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Expiration { get; set; }
    }
}
