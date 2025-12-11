// Models/Categoria.cs

namespace EcommerceApi.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        // Soporte para jerarquía (categoría padre)
        public int? CategoriaPadreId { get; set; }

        // Propiedad de Navegación a la categoría padre
        public Categoria? CategoriaPadre { get; set; }

        // Propiedad de Navegación a las subcategorías (hijos)
        public ICollection<Categoria>? SubCategorias { get; set; }

        // Relación N:M con Artículos
        public ICollection<ArticuloCategoria>? ArticuloCategorias { get; set; }
    }
}