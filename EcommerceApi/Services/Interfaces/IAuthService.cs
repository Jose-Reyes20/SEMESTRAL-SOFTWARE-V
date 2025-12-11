using EcommerceApi.DTOs; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO

namespace EcommerceApi.Services.Interfaces // <<-- CORREGIDO
{
    public interface IAuthService
    {
        // Se corrige el retorno a Usuario, que es la entidad de autenticación real.
        // Se asume que LoginDto contiene los campos de inicio de sesión (Correo y Password/Contrasena).
        Task<Usuario?> Login(LoginDto dto);
    }
}