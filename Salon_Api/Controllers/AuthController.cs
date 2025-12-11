using Microsoft.AspNetCore.Mvc;
using Salon_Api.DTO;
using Salon_Api.Services.Interfaces;

namespace Salon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var cliente = await _authService.Login(dto);

                if (cliente == null)
                    return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });

                // Determine role based on email
                string rol = cliente.Correo == "admin@matcha.com" ? "admin" : "usuario";

                return Ok(new
                {
                    id = cliente.IdCliente,
                    nombre = cliente.Nombre,
                    correo = cliente.Correo,
                    rol = rol
                });
            }
            catch (Exception)
            {
                // Mensaje genérico para el usuario
                return BadRequest(new { mensaje = "Ocurrió un error al intentar iniciar sesión." });
            }
        }
    }
}