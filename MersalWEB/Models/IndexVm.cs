namespace MersalWEB.Models
{
    public class IndexVm
    {



        public IndexVm()
        {

            Orders = new List<Order>();
            products = new List<Product>();
            Categories = new List<Category>();
            OrderDetails = new List<OrderDetail>();
            ContactAdmins= new List<ContactAdmin>();
            contacts = new List<Contact>();

        }

        public List<Order> Orders { get; set; }
        public List<Product> products { get; set; }
        public List<Product> Products { get; internal set; }
        public List<OrderDetail> OrderDetails{ get; set;}
        public List<Category> Categories{ get; set;}
        public List<ContactAdmin> ContactAdmins { get; set; }
        public List<Contact> contacts { get; set; }
        public List<Review> Reviews { get; internal set; }
        public List<ProductImage> ProductImages { get; internal set; }
    }
}
