using Microsoft.EntityFrameworkCore;
using Salon_Api.Data;
using Salon_Api.Modelo;
using Salon_Api.Services.Interfaces;
using Salon_Api.DTO;

namespace Salon_Api.Services
{
    public class CuponesService : ICuponesService
    {
        private readonly ApplicationDbContext _context;

        public CuponesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CuponDto?> GetCuponPorCodigo(string codigo)
        {
            var c = await _context.Cupones.FirstOrDefaultAsync(x => x.Codigo == codigo);
            if (c == null) return null;
            return new CuponDto
            {
                Id = c.Id,
                Codigo = c.Codigo,
                Descuento = c.Descuento,
                Estado = c.Estado
            };
        }

        public async Task<CuponDto> CrearCupon(Cupon cupon)
        {
            _context.Cupones.Add(cupon);
            await _context.SaveChangesAsync();
            return new CuponDto
            {
                Id = cupon.Id,
                Codigo = cupon.Codigo,
                Descuento = cupon.Descuento,
                Estado = cupon.Estado
            };
        }

        public async Task<CuponDto?> ActualizarCupon(int id, Cupon cupon)
        {
            var existente = await _context.Cupones.FindAsync(id);
            if (existente == null) return null;
            existente.Codigo = cupon.Codigo;
            existente.Descuento = cupon.Descuento;
            existente.Estado = cupon.Estado;
            await _context.SaveChangesAsync();
            return new CuponDto
            {
                Id = existente.Id,
                Codigo = existente.Codigo,
                Descuento = existente.Descuento,
                Estado = existente.Estado
            };
        }
    }
}
