using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Models;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.DTOs;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _clientesService;

        public ClientesController(IClientesService clientesService)
        {
            _clientesService = clientesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _clientesService.ObtenerClientes();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            var cliente = await _clientesService.ObtenerClientePorId(id);
            if (cliente == null) return NotFound(new { mensaje = "Cliente no encontrado." });
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteCreateDto dto)
        {
            try
            {
                var clienteCreado = await _clientesService.CrearCliente(dto);
                // CORREGIDO: Se usa 'Id' en lugar de 'IdCliente'
                return CreatedAtAction(nameof(GetCliente), new { id = clienteCreado.Id }, clienteCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ClienteCreateDto dto)
        {
            var clienteActualizado = await _clientesService.ActualizarCliente(id, dto);
            if (clienteActualizado == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var result = await _clientesService.EliminarCliente(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}