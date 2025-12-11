// ClienteCreateDto.cs (Corregido)

using System.ComponentModel.DataAnnotations; // Recomendado para validaciones

namespace EcommerceApi.DTOs // <<-- CORREGIDO
{
    public class ClienteCreateDto
    {
        [Required]
        public string Nombre { get; set; } = null!;

        // Sugerencia: Añadir validaciones [Required] si aplican
        public string? Telefono { get; set; }
        public string? Correo { get; set; }

        [Required]
        public string Password { get; set; } = null!; // Contraseña para hashear

        // Se ha ELIMINADO: public DateTime FechaRegistro { get; set; }
    }
}