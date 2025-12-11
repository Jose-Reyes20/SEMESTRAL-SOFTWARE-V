// DTOs/UserLoginDto.cs

using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.DTOs
{
    public class UserLoginDto
    {
        [Required] public string Usuario { get; set; } // Correo o nombre de usuario
        [Required] public string Contrasena { get; set; }
    }
}