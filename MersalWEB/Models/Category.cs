using static NuGet.Packaging.PackagingConstants;

namespace MersalWEB.Models
{
    public class Category
    {
        public Category()
        {
            
            products = new HashSet<Product>();
        }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public string? Photo { get; set; }
        public virtual ICollection<Product> products { get; set; }
    }
}
