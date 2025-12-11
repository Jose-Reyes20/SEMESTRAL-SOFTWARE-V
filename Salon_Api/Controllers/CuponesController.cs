using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;
using Salon_Api.Modelo;

namespace Salon_Api.Controllers
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

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetPorCodigo(string codigo)
        {
            var cupon = await _service.GetCuponPorCodigo(codigo);
            if (cupon == null) return NotFound();
            return Ok(cupon);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCupon([FromBody] Cupon cupon)
        {
            var created = await _service.CrearCupon(cupon);
            return CreatedAtAction(nameof(GetPorCodigo), new { codigo = created.Codigo }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cupon cupon)
        {
            var updated = await _service.ActualizarCupon(id, cupon);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
