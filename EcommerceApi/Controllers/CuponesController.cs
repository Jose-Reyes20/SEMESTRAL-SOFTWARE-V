using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO
using EcommerceApi.DTOs; // <<-- AGREGADO para asegurar la disponibilidad de los DTOs

namespace EcommerceApi.Controllers // <<-- CORREGIDO
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuponesController : ControllerBase
    {
        private readonly ICuponesService _service;

        public CuponesController(ICuponesService service)
        {
            _service = service;
        }

        // GET /api/cupones/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetPorCodigo(string codigo)
        {
            // El servicio retorna CuponDto
            var cupon = await _service.GetCuponPorCodigo(codigo);
            if (cupon == null) return NotFound();
            return Ok(cupon);
        }

        // POST /api/cupones
        [HttpPost]
        public async Task<IActionResult> CrearCupon([FromBody] Cupon cupon)
        {
            // Nota: Aquí estás usando el Modelo (Cupon) para recibir datos, 
            // lo ideal sería usar un DTO (CuponCreateDto) para validación.
            var created = await _service.CrearCupon(cupon);
            // created retorna un CuponDto (según tu ICuponesService.cs)
            return CreatedAtAction(nameof(GetPorCodigo), new { codigo = created.Codigo }, created);
        }

        // PUT /api/cupones/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cupon cupon)
        {
            // Nota: Igual que antes, usando Modelo (Cupon) en vez de DTO para la entrada.
            var updated = await _service.ActualizarCupon(id, cupon);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}