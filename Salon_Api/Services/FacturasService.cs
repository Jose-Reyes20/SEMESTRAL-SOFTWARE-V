using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;

namespace Salon_Api.Services
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
