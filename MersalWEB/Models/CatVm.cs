namespace MersalWEB.Models
{
    public class CatVm
    {
        public CatVm()
        {
            category = new Category();

        }
        public IFormFile? CatImgVm { get; set; }
        public Category category { get; set; }
    }
}
