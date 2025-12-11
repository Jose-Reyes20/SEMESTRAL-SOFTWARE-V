// Services/AuthService.cs

using EcommerceApi.DTOs;
using EcommerceApi.Models;

namespace EcommerceApi.Services
{
    public class AuthService
    {
        // Simulación de contexto de base de datos
        private readonly List<Cliente> _clientes = new List<Cliente>();
        private readonly List<Usuario> _usuarios = new List<Usuario>();

        // Simula la creación del cliente y el usuario
        public Usuario Register(UserRegisterDto dto)
        {
            // Lógica real: Verificar si el correo ya existe.

            // 1. Crear Cliente
            var nuevoCliente = new Cliente
            {
                Id = _clientes.Count + 1,
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                // ... otros campos
            };
            _clientes.Add(nuevoCliente);

            // 2. Crear Usuario
            var nuevoUsuario = new Usuario
            {
                Id = _usuarios.Count + 1,
                Username = dto.Correo,
                Contrasena = "HASHED_" + dto.Contrasena, // Lógica real: Usar BCrypt/Argon2
                Rol = "cliente",
                ClienteId = nuevoCliente.Id
            };
            _usuarios.Add(nuevoUsuario);

            return nuevoUsuario;
        }

        // Simula el proceso de inicio de sesión
        public Usuario? Login(UserLoginDto dto)
        {
            // Lógica real: 
            // 1. Buscar el usuario por 'dto.Usuario'.
            // 2. Comparar el hash de 'dto.Contrasena' con el hash almacenado.

            var usuarioEncontrado = _usuarios.FirstOrDefault(u => u.Username == dto.Usuario);

            if (usuarioEncontrado != null && usuarioEncontrado.Contrasena == ("HASHED_" + dto.Contrasena))
            {
                // Lógica real: Generar y devolver un JWT (JSON Web Token)
                return usuarioEncontrado;
            }

            return null; // Fallo de autenticación
        }
    }
}