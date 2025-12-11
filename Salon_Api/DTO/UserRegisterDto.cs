// DTOs/UserRegisterDto.cs

using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.DTOs
{
    public class UserRegisterDto
    {
        // Datos del Cliente
        [Required] public string Nombre { get; set; }
        [Required] public string Apellido { get; set; }
        [Required] public string Direccion { get; set; }
        [Required] public string Telefono { get; set; }

        // Datos de la Cuenta de Usuario
        [Required][EmailAddress] public string Correo { get; set; }
        [Required][MinLength(6)] public string Contrasena { get; set; }
    }
}