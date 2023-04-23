namespace MersalWEB.Models
{
    public class ProImagesVm
    {
        public ProImagesVm()
        {
            product = new Product();
            productimages = new List <ProductImage>();

        }
        public List<IFormFile> proImgsVm { get; set; }
        public IFormFile prophoto { get; set; }
        public string CatName { get; set; }
        public Product product { get; set; }
        public List <ProductImage> productimages { get; set; }
    }
}
