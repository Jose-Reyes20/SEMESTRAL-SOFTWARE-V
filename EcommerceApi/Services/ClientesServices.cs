using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data; // <<-- CORREGIDO
using EcommerceApi.DTOs; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO (Para la entidad Cliente)
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO
using BCrypt.Net;

namespace EcommerceApi.Services // <<-- CORREGIDO
{
    // Se corrige el nombre de la clase a ClienteService (singular) para consistencia
    public class ClientesService : IClientesService
    {
        private readonly ApplicationDbContext _context;

        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Se cambia el tipo de retorno a 'Cliente' (singular)
        public async Task<IEnumerable<Cliente>> ObtenerClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // Se cambia el tipo de retorno a 'Cliente' (singular)
        public async Task<Cliente?> ObtenerClientePorId(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        // ✅ Crear Cliente con DTO
        // Se cambia el tipo de retorno a 'Cliente' (singular)
        public async Task<Cliente> CrearCliente(ClienteCreateDto dto)
        {
            // Validación: solo números en Telefono
            if (!dto.Telefono.All(char.IsDigit))
                throw new Exception("El teléfono solo puede contener números.");

            // El modelo Cliente debe tener una propiedad 'PasswordHash'
            var nuevoCliente = new Cliente
            {
                // NOTA: Si el modelo Cliente no tiene Apellido y Direccion, esto podría causar un error.
                Nombre = dto.Nombre,
                // FechaRegistro eliminada del DTO, la DB/EF se encarga de CreatedAt
                Telefono = dto.Telefono,
                Correo = dto.Correo,

                // 👇 Hashear la contraseña ANTES de guardar
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return nuevoCliente;
        }

        // ✅ Actualizar Cliente con DTO
        // Se cambia el tipo de retorno a 'Cliente' (singular)
        public async Task<Cliente?> ActualizarCliente(int id, ClienteCreateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return null;

            // Validación: solo números en Telefono
            if (!dto.Telefono.All(char.IsDigit))
                throw new Exception("El teléfono solo puede contener números.");

            cliente.Nombre = dto.Nombre;
            cliente.Telefono = dto.Telefono;
            cliente.Correo = dto.Correo;
            // cliente.FechaRegistro = dto.FechaRegistro; // <-- Campo FechaRegistro eliminado del DTO.

            // IMPORTANTE: No se debe actualizar el hash de la contraseña (PasswordHash) 
            // en una actualización de datos simple, sino mediante un endpoint específico.

            await _context.SaveChangesAsync();

            return cliente;
        }

        public async Task<bool> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}