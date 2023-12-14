using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lab13C.Models;

namespace Lab13C.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClienteCustomer : ControllerBase
    {
        private readonly InvoiceContext _context;
        
        public ClienteCustomer(InvoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListarClientes(string nombre, string apellido)
        {
            // Consulta para filtrar y ordenar los clientes
            var clientesFiltrados = _context.Customers
                .Where(cliente =>
                    (string.IsNullOrEmpty(nombre) || cliente.FirstName.Contains(nombre)) &&
                    (string.IsNullOrEmpty(apellido) || cliente.LastName.Contains(apellido)) 
                )
                .OrderByDescending(cliente => cliente.LastName)
                .ToList();

            if (clientesFiltrados.Any())
            {
                // Devuelve los clientes encontrados
                return Ok(clientesFiltrados);
            }

            // Devuelve un resultado NotFound si no se encuentran clientes
            return NotFound("No se encontraron los clientes.");
        }



    }
}
