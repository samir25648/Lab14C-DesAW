namespace Lab13C.Models.Request
{
    public class InvoiceRequestV1
    {
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }

        public decimal Total { get; set; }

    }
}
