using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data; // <<-- CORREGIDO (Para ApplicationDbContext)
using EcommerceApi.Models; // <<-- CORREGIDO (Para la entidad Cupon)
using EcommerceApi.Services.Interfaces; // <<-- CORREGIDO (Para ICuponesService)
using EcommerceApi.DTOs; // <<-- CORREGIDO (Para CuponDto)

namespace EcommerceApi.Services // <<-- CORREGIDO
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
            // Nota: Aquí podrías querer actualizar también los campos CreatedAt/UpdatedAt
            // existente.UpdatedAt = DateTime.UtcNow; 
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
