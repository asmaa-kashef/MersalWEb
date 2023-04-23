using MersalWEB.Models;
using System;
using System.Collections.Generic;

namespace MersalWEB.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? OrderdetailsId { get; set; }
        public int? ProductId { get; set; }
       public virtual OrderDetail? Orderdetails { get; set; }
        public virtual Product? Product { get; set; }
    }
}
