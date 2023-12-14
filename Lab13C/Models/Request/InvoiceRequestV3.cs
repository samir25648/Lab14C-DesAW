namespace Lab13C.Models.Request
{
    public class InvoiceRequestV3
    {
        public int InvoiceId { get; set; }
        public List<DetailRequestV1> Details { get; set; }
    }

    public class DetailRequestV1
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
