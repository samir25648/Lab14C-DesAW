using Lab13C.Models;
using Lab13C.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab13C.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceCustomController : ControllerBase
    {
        private readonly InvoiceContext _context;

        public InvoiceCustomController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> Insert(InvoiceRequestV1 request)
        {
            try
            {
                var invoice = new Invoice
                {
                    Date = request.Date,
                    InvoiceNumber = request.InvoiceNumber,
                    Total = request.Total,
                    CustomerId = request.CustomerId,
                };

                if (_context.Invoices == null)
                {
                    return Problem("Entity set 'Context.Invoices' is null.");
                }

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetInvoice", new { id = invoice.CustomerId }, invoice);
            }
            catch (Exception ex)
            {
                return Problem($"Error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> InsertList(InvoiceRequestV2 request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var customer = await _context.Customers.FindAsync(request.CustomerId);

                    if (customer == null)
                    {
                        return NotFound($"Customer with ID {request.CustomerId} not found.");
                    }

                    if (_context.Invoices == null)
                    {
                        return Problem("Entity set 'Context.Invoices' is null.");
                    }

                    foreach (var reqInvoice in request.Invoices)
                    {
                        var invoice = new Invoice
                        {
                            CustomerId = request.CustomerId,
                            Date = reqInvoice.Date,
                            InvoiceNumber = reqInvoice.InvoiceNumber,
                            Total = reqInvoice.Total
                        };

                        _context.Invoices.Add(invoice);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Problem($"Error: {ex.Message}");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertInvoiceDetail(InvoiceRequestV3 request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoice = await _context.Invoices.FindAsync(request.InvoiceId);

                    if (invoice == null)
                    {
                        return NotFound($"Invoice with ID {request.InvoiceId} not found.");
                    }

                    if (_context.Details == null || _context.Products == null)
                    {
                        return Problem("Entity sets 'Context.Details' or 'Context.Products' are null.");
                    }

                    foreach (var reqDetail in request.Details)
                    {
                        var product = await _context.Products.FindAsync(reqDetail.ProductId);

                        if (product == null)
                        {
                            return NotFound($"Product with ID {reqDetail.ProductId} not found.");
                        }

                        var detail = new Detail
                        {
                            ProductId = reqDetail.ProductId,
                            Amount = reqDetail.Amount,
                            Price = reqDetail.Price,
                            SubTotal = reqDetail.Amount * reqDetail.Price,
                            Product = product,  // Asociar el producto al detalle
                            Invoice = invoice    // Asociar la factura al detalle
                        };

                        _context.Details.Add(detail);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Problem($"Error: {ex.Message}");
                }
            }
        }


    }
}
