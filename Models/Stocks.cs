using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Models
{
    public class Stocks
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Cost { get; set; }
        [Required]
        public DateTime Expiration { get; set; }

        public Сonditions State { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Values that represent сonditions. </summary>
        ///
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public enum Сonditions
        {
            Default,
            Less2MonthExpiration,
            Expired
        }
    }
}
