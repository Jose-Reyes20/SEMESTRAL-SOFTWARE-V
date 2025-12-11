using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;

namespace Salon_Api.Services
{
    public class OrdenesService : IOrdenesService
    {
        private readonly ApplicationDbContext _context;

        public OrdenesService(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<OrdenDto> CrearOrden(Orden orden)
        {
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

        public async Task<bool> ActualizarEstado(int id, string nuevoEstado)
        {
            var orden = await _context.Ordenes.FindAsync(id);
            if (orden == null) return false;
            orden.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
