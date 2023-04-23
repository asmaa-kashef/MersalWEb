using MersalWEB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MersalWEB.Models
{
    public class Product
    {
        public Product()
        {
            Invoices = new HashSet<Invoice>();
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            ProductImages = new HashSet<ProductImage>();
        }

        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public string? Photo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? SupplierName { get; set; }
        public int? CategoryId { get; set; }
        public bool IsAvailable { get; set; }
        public string? ReviewUrl { get; set; }
        [NotMapped]
        public Category category { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }

      




    }
}
