namespace MersalWEB.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int? Number { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Qty { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? ProductId { get; set; }
       
        public virtual Product? Product { get; set; }
       




    }
}
