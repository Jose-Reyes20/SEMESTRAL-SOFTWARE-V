using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data; // <<-- CORREGIDO (Para ApplicationDbContext)
using EcommerceApi.Models; // <<-- CORREGIDO (Para la entidad Factura)
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO (Para IFacturasService)
using EcommerceApi.DTOs; // <<-- CORREGIDO (Para FacturaDto)

namespace EcommerceApi.Services // <<-- CORREGIDO
{
    public class FacturasService : IFacturasService
    {
        private readonly ApplicationDbContext _context;

        public FacturasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FacturaDto>> GetFacturas()
        {
            return await _context.Facturas
                .Select(f => new FacturaDto
                {
                    Id = f.Id,
                    CuponId = f.CuponId,
                    Subtotal = f.Subtotal,
                    Descuento = f.Descuento,
                    Total = f.Total,
                    Fecha = f.Fecha,
                    Itbms = f.Itbms,
                    UsuarioId = f.UsuarioId
                })
                .ToListAsync();
        }

        public async Task<FacturaDto?> GetFacturaPorId(int id)
        {
            var f = await _context.Facturas.FindAsync(id);
            if (f == null) return null;
            return new FacturaDto
            {
                Id = f.Id,
                CuponId = f.CuponId,
                Subtotal = f.Subtotal,
                Descuento = f.Descuento,
                Total = f.Total,
                Fecha = f.Fecha,
                Itbms = f.Itbms,
                UsuarioId = f.UsuarioId
            };
        }
    }
}