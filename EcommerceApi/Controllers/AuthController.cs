// Controllers/AuthController.cs

using Microsoft.AspNetCore.Mvc;
using EcommerceApi.DTOs;
using EcommerceApi.Services;

namespace EcommerceApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevoUsuario = _authService.Register(registerDto);

            return CreatedAtAction(nameof(Register), new { message = "Registro exitoso", userId = nuevoUsuario.Id });
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = _authService.Login(loginDto);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" }); // 401 Unauthorized
            }

            // Retorna la información necesaria para el cliente (token y datos básicos)
            return Ok(new
            {
                Token = "GENERATED_JWT_TOKEN", // Aquí iría el JWT real
                UsuarioId = usuario.Id,
                Rol = usuario.Rol,
                ClienteId = usuario.ClienteId
            });
        }
    }
}