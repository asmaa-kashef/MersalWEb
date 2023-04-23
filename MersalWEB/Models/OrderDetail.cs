using System;
using System.Collections.Generic;

namespace MersalWEB.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            Orders = new HashSet<Order>();
        }

        public int OrderDetailId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool? IsOnline { get; set; }
          public virtual ICollection<Order> Orders { get; set; }
    }
}
