// Services/ArticuloService.cs

using EcommerceApi.DTOs;
using EcommerceApi.Models;

namespace EcommerceApi.Services
{
    public class ArticuloService
    {
        // Esta es una simulación del repositorio/DB context
        private readonly List<Articulo> _articulos = new List<Articulo>();
        private int _nextId = 1;

        // Simula la creación del artículo (POST)
        public Articulo CreateArticulo(ArticuloCreateDto dto)
        {
            var nuevoArticulo = new Articulo
            {
                Id = _nextId++,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                PagaItbms = dto.PagaItbms,
                // Lógica real: Aquí se guardarían las categorías y fotos en sus tablas respectivas
            };

            _articulos.Add(nuevoArticulo);
            return nuevoArticulo;
        }

        // Simula la obtención de todos los artículos (GET)
        public IEnumerable<Articulo> GetAllArticulos()
        {
            // Lógica real: Aquí harías el JOIN con categorías y fotos
            return _articulos.AsEnumerable();
        }
    }
}