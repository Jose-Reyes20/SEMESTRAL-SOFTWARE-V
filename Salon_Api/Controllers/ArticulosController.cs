// Controllers/ArticulosController.cs

using Microsoft.AspNetCore.Mvc;
using EcommerceApi.DTOs;
using EcommerceApi.Services;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly ArticuloService _articuloService;

        // Inyección de dependencia (se configura en Program.cs)
        public ArticulosController(ArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

        // POST /api/articulos
        [HttpPost]
        public IActionResult CrearArticulo([FromBody] ArticuloCreateDto articuloDto)
        {
            // La validación del DTO (Required, MaxLength, etc.) la hace ASP.NET Core automáticamente
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevoArticulo = _articuloService.CreateArticulo(articuloDto);

            // Retorna 201 Created y la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetArticuloById), new { id = nuevoArticulo.Id }, nuevoArticulo);
        }

        // GET /api/articulos
        [HttpGet]
        public IActionResult GetArticulos()
        {
            var articulos = _articuloService.GetAllArticulos();
            return Ok(articulos); // Retorna 200 OK
        }

        // GET /api/articulos/{id}
        [HttpGet("{id}")]
        public IActionResult GetArticuloById(int id)
        {
            // Lógica para buscar por ID
            // ...
            return Ok(new { id = id, nombre = "Articulo de Prueba" });
        }
    }
}