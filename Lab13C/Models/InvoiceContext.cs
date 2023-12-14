using Microsoft.EntityFrameworkCore;

namespace Lab13C.Models
{
    public class InvoiceContext : DbContext
    {
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public InvoiceContext(DbContextOptions<InvoiceContext> options)
         : base(options)
        {

        }
    }
}
