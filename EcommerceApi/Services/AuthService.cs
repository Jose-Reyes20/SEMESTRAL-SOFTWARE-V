using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using BCrypt.Net;

namespace EcommerceApi.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario Register(UserRegisterDto dto)
        {
            // 1. Crear Cliente
            var nuevoCliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                Correo = dto.Correo
            };

            // Guardamos primero el cliente para obtener su ID
            _context.Clientes.Add(nuevoCliente);
            _context.SaveChanges();

            // 2. Crear Usuario vinculado
            var nuevoUsuario = new Usuario
            {
                Username = dto.Correo,
                // Encriptamos la contraseña
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Rol = "cliente",
                ClienteId = nuevoCliente.Id // Usamos el ID generado
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            return nuevoUsuario;
        }

        public Usuario? Login(UserLoginDto dto)
        {
            // Buscamos el usuario por Username/Correo
            var usuarioEncontrado = _context.Usuarios
                .FirstOrDefault(u => u.Username == dto.Usuario);

            if (usuarioEncontrado == null) return null;

            // Verificamos si la contraseña coincide con el hash
            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuarioEncontrado.Contrasena);

            if (validPassword)
            {
                return usuarioEncontrado;
            }

            return null;
        }
    }
}