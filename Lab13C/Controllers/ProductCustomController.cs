using Lab13C.Models.Request;
using Lab13C.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;

namespace Lab13C.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductCustomController : ControllerBase
    {
        private readonly InvoiceContext _context;

        //Objeto en el constructor como parámetro
        //Inyección de dependencias
        public ProductCustomController(InvoiceContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            return await _context.Customers.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Insert(ProductRequestV1 request)
        {
            try
            {
                //Convertir request=>model
                Product model = new Product();
                model.Price = request.Price;
                model.Name = request.Name;
                model.IsActive = true;


                _context.Products.Add(model);
                await _context.SaveChangesAsync();//Confirmación o commit
                return CreatedAtAction("GetProduct", new { id = model.ProductId }, model);
            }
            catch (Exception ex)
            {
                return Problem($"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ProductRequestV2 request)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(request.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            product.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductRequestV3 request)
        {
            try
            {
                var product = await _context.Products.FindAsync(request.ProductId);

                if (product == null)
                {
                    return NotFound();
                }

                // Actualizar las propiedades según la solicitud
                product.Price = request.Price;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem($"Error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProductList(ProductRequestV4 request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_context.Products == null)
                    {
                        return Problem("Entity set 'Context.Products' is null.");
                    }

                    var productsToDelete = await _context.Products
                        .Where(p => request.ListProducts.Contains(p.ProductId))
                        .ToListAsync();

                    if (productsToDelete == null || !productsToDelete.Any())
                    {
                        return NotFound("No products found for deletion.");
                    }


                    foreach (var product in productsToDelete)
                    {
                        product.IsActive = false;
                    }

                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Ok("Products deactivated successfully.");
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
