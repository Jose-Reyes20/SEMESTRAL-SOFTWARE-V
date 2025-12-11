using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO
using EcommerceApi.DTOs; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO

namespace EcommerceApi.Controllers // <<-- CORREGIDO
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdenesController : ControllerBase
    {
        private readonly IOrdenesService _service;

        public OrdenesController(IOrdenesService service)
        {
            _service = service;
        }

        // GET /api/ordenes (Para administradores)
        [HttpGet]
        public async Task<IActionResult> GetOrdenes()
        {
            var list = await _service.GetOrdenes();
            return Ok(list);
        }

        // GET /api/usuarios/{usuarioId}/ordenes (Para clientes)
        // Nota: La ruta de atributo ya incluye la ruta base 'api/ordenes'. 
        // Para que esta ruta anidada funcione, se debe usar la sintaxis [Route("~/api/usuarios/{usuarioId}/ordenes")] o [HttpGet("/api/usuarios/{usuarioId}/ordenes")].
        [HttpGet("/api/usuarios/{usuarioId}/ordenes")]
        public async Task<IActionResult> GetOrdenesPorUsuario(int usuarioId)
        {
            var list = await _service.GetOrdenesPorUsuario(usuarioId);
            return Ok(list);
        }

        // POST /api/ordenes
        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] Orden orden)
        {
            // NOTA: Se recomienda usar un DTO (OrdenCreateDto) en lugar del modelo 'Orden' para la entrada.
            var created = await _service.CrearOrden(orden);
            // created retorna un OrdenDto (según tu IOrdenesService.cs)
            return CreatedAtAction(nameof(GetOrdenById), new { id = created.Id }, created);
        }

        // GET /api/ordenes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdenById(int id)
        {
            var orden = await _service.GetOrdenPorId(id);
            if (orden == null) return NotFound();
            return Ok(orden);
        }

        // PUT /api/ordenes/{id}/estado
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] string nuevoEstado)
        {
            var ok = await _service.ActualizarEstado(id, nuevoEstado);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}