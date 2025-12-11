using Microsoft.AspNetCore.Mvc;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetFacturas()
        {
            var list = await _service.GetFacturas();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacturaById(int id)
        {
            var f = await _service.GetFacturaPorId(id);
            if (f == null) return NotFound();
            return Ok(f);
        }
    }
}
