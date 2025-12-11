using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Services
{
    public class ClientesService : IClientesService
    {
        private readonly ApplicationDbContext _context;

        public ClientesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ObtenerClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> ObtenerClientePorId(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente> CrearCliente(ClienteCreateDto dto)
        {
            // Validación básica
            if (string.IsNullOrEmpty(dto.Telefono) || !dto.Telefono.All(char.IsDigit))
                throw new Exception("El teléfono solo puede contener números.");

            var nuevoCliente = new Cliente
            {
                Nombre = dto.Nombre,
                // Si tu DTO no tiene Apellido/Dirección, usa valores por defecto o string.Empty
                Apellido = "",
                Direccion = "",
                Telefono = dto.Telefono,
                Correo = dto.Correo ?? ""
                // NOTA: No guardamos Password aquí. El password es para el Usuario (Login).
                // Si necesitas crear el usuario login al mismo tiempo, usa AuthService.
            };

            _context.Clientes.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return nuevoCliente;
        }

        public async Task<Cliente?> ActualizarCliente(int id, ClienteCreateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            if (!string.IsNullOrEmpty(dto.Telefono) && !dto.Telefono.All(char.IsDigit))
                throw new Exception("El teléfono solo puede contener números.");

            cliente.Nombre = dto.Nombre;
            if (dto.Telefono != null) cliente.Telefono = dto.Telefono;
            if (dto.Correo != null) cliente.Correo = dto.Correo;

            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}