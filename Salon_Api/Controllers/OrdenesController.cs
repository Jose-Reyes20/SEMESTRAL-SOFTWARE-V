using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetOrdenes()
        {
            var list = await _service.GetOrdenes();
            return Ok(list);
        }

        [HttpGet("/api/usuarios/{usuarioId}/ordenes")]
        public async Task<IActionResult> GetOrdenesPorUsuario(int usuarioId)
        {
            var list = await _service.GetOrdenesPorUsuario(usuarioId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] Orden orden)
        {
            var created = await _service.CrearOrden(orden);
            return CreatedAtAction(nameof(GetOrdenById), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdenById(int id)
        {
            var orden = await _service.GetOrdenPorId(id);
            if (orden == null) return NotFound();
            return Ok(orden);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] string nuevoEstado)
        {
            var ok = await _service.ActualizarEstado(id, nuevoEstado);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
