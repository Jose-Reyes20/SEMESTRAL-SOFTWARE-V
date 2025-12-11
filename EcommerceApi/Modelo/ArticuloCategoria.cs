// Models/ArticuloCategoria.cs

namespace EcommerceApi.Models
{
    public class ArticuloCategoria
    {
        // Llave primaria (si usas la tabla de enlace)
        public int Id { get; set; }

        public int IdArticulo { get; set; }
        public Articulo Articulo { get; set; } // Propiedad de Navegación

        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; } // Propiedad de Navegación
    }
}