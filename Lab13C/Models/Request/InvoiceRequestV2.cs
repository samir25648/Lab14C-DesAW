namespace Lab13C.Models.Request
{
    public class InvoiceRequestV2
    {
        public int CustomerId { get; set; }
        public List<InvoiceV2> Invoices { get; set; }
    }

    public class InvoiceV2
    {
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Total { get; set; }
    }
}
