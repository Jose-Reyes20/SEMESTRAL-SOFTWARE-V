using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO: De Salon_Api.Services.Interfaces a EcommerceApi.Services.Interfaces
// using EcommerceApi.DTOs; // Podría ser necesario si IFacturasService usa DTOs directamente aquí

namespace EcommerceApi.Controllers // <<-- CORREGIDO: De Salon_Api.Controllers a EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturasService _service;

        public FacturasController(IFacturasService service)
        {
            _service = service;
        }

        // GET /api/facturas
        [HttpGet]
        public async Task<IActionResult> GetFacturas()
        {
            var list = await _service.GetFacturas();
            return Ok(list);
        }

        // GET /api/facturas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacturaById(int id)
        {
            var f = await _service.GetFacturaPorId(id);
            if (f == null) return NotFound();
            return Ok(f);
        }
    }
}