using Lab13C.Models;
using Lab13C.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab13C.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerCustomController : ControllerBase
    {
        private readonly InvoiceContext _context;

        //Objeto en el constructor como parámetro
        //Inyección de dependencias
        public CustomerCustomController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Insert(CustomerRequestV1 request)
        {
            try
            {
                if (_context.Customers == null)
                {
                    return Problem("Entity set 'Context.Customers' is null.");
                }

                var customer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DocumentNumber = request.DocumentNumber,
                    IsActive = true
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
            }
            catch (DbUpdateException dbEx)
            {
                // Manejar la excepción específica relacionada con la base de datos
                return Problem($"Error al actualizar la base de datos: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones de manera genérica
                return Problem($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(CustomerRequestV2 request)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(request.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            customer.IsActive = false;
            //_context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update(CustomerRequestV3 request)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(request.CustomerId);

                if (customer == null)
                {
                    return NotFound();
                }

                // Actualizar las propiedades según la solicitud
                customer.DocumentNumber = request.DocumentNumber;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem($"Error: {ex.Message}");
            }
        }

    }
}
