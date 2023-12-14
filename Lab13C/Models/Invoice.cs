namespace Lab13C.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Total { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
