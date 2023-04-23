using MersalWEB.Models;
using System;
using System.Collections.Generic;

namespace MersalWEB.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int Qty { get; set; }
        public decimal? Price { get; set; }
        public string? UserId { get; set; }
        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }
       
    }
}
