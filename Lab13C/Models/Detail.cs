namespace Lab13C.Models
{
    public class Detail
    {
        public int DetailId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

    }
}
