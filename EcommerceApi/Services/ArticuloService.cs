using Microsoft.EntityFrameworkCore;
using EcommerceApi.Data;
using EcommerceApi.DTOs;
using EcommerceApi.Models;

namespace EcommerceApi.Services
{
    public class ArticuloService
    {
        private readonly ApplicationDbContext _context;

        public ArticuloService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Articulo CreateArticulo(ArticuloCreateDto dto)
        {
            var nuevoArticulo = new Articulo
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                PagaItbms = dto.PagaItbms
                // Nota: La lógica para guardar categorías y fotos requeriría más código aquí
            };

            _context.Articulos.Add(nuevoArticulo);
            _context.SaveChanges();
            return nuevoArticulo;
        }

        public IEnumerable<Articulo> GetAllArticulos()
        {
            return _context.Articulos.ToList();
        }
    }
}