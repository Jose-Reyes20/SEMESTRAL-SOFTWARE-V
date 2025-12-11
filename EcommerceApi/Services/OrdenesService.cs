using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO (Para la entidad Orden)
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO (Para IOrdenesService)
using EcommerceApi.DTOs; // <<-- CORREGIDO (Para OrdenDto)

namespace EcommerceApi.Services // <<-- CORREGIDO
{
    public class OrdenesService : IOrdenesService
    {
        private readonly ApplicationDbContext _context;

        public OrdenesService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las órdenes (Administrador)
        public async Task<IEnumerable<OrdenDto>> GetOrdenes()
        {
            return await _context.Ordenes
                .Select(o => new OrdenDto
                {
                    Id = o.Id,
                    Estado = o.Estado,
                    Fecha = o.Fecha,
                    UsuarioId = o.UsuarioId,
                    CuponId = o.CuponId,
                    Subtotal = o.Subtotal,
                    Total = o.Total,
                    Descuento = o.Descuento,
                    Itbms = o.Itbms
                })
                .ToListAsync();
        }

        // Obtener órdenes por usuario (Cliente)
        public async Task<IEnumerable<OrdenDto>> GetOrdenesPorUsuario(int usuarioId)
        {
            return await _context.Ordenes
                .Where(o => o.UsuarioId == usuarioId)
                .Select(o => new OrdenDto
                {
                    Id = o.Id,
                    Estado = o.Estado,
                    Fecha = o.Fecha,
                    UsuarioId = o.UsuarioId,
                    CuponId = o.CuponId,
                    Subtotal = o.Subtotal,
                    Total = o.Total,
                    Descuento = o.Descuento,
                    Itbms = o.Itbms
                })
                .ToListAsync();
        }

        // Crear Orden
        public async Task<OrdenDto> CrearOrden(Orden orden)
        {
            // NOTA: Se recomienda encarecidamente usar un DTO (OrdenCreateDto) 
            // en lugar del modelo 'Orden' como parámetro de entrada para asegurar que 
            // el cliente no pueda sobrescribir campos protegidos como el Total o ITBMS.
            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();

            return new OrdenDto
            {
                Id = orden.Id,
                Estado = orden.Estado,
                Fecha = orden.Fecha,
                UsuarioId = orden.UsuarioId,
                CuponId = orden.CuponId,
                Subtotal = orden.Subtotal,
                Total = orden.Total,
                Descuento = orden.Descuento,
                Itbms = orden.Itbms
            };
        }

        // Obtener Orden por ID
        public async Task<OrdenDto?> GetOrdenPorId(int id)
        {
            var o = await _context.Ordenes.FindAsync(id);
            if (o == null) return null;
            return new OrdenDto
            {
                Id = o.Id,
                Estado = o.Estado,
                Fecha = o.Fecha,
                UsuarioId = o.UsuarioId,
                CuponId = o.CuponId,
                Subtotal = o.Subtotal,
                Total = o.Total,
                Descuento = o.Descuento,
                Itbms = o.Itbms
            };
        }

        // Actualizar Estado de la Orden
        public async Task<bool> ActualizarEstado(int id, string nuevoEstado)
        {
            var orden = await _context.Ordenes.FindAsync(id);
            if (orden == null) return false;

            // Lógica de negocio adicional (ej: validar si el nuevoEstado es válido)
            orden.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}