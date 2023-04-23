using System;
using System.Collections.Generic;

namespace MersalWEB.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
  
    }
}
