using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestaColombia.Infrastructure.Data;
using PrestaColombia.Domain.Entities;

namespace PrestaColombia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly PrestaColombiaDbContext _context;

        public ClientesController(PrestaColombiaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientes = await _context.Clientes
                .Where(c => c.Estado)
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null || !cliente.Estado)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            cliente.Id = Guid.NewGuid();
            cliente.FechaCreacion = DateTime.UtcNow;
            cliente.Estado = true;

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Cliente cliente)
        {
            var clienteDb = await _context.Clientes.FindAsync(id);

            if (clienteDb == null)
                return NotFound();

            clienteDb.Nombre = cliente.Nombre;
            clienteDb.Documento = cliente.Documento;
            clienteDb.Telefono = cliente.Telefono;
            clienteDb.Direccion = cliente.Direccion;

            await _context.SaveChangesAsync();

            return Ok(clienteDb);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            cliente.Estado = false; // Soft delete

            await _context.SaveChangesAsync();

            return Ok("Cliente eliminado");
        }
    }
}