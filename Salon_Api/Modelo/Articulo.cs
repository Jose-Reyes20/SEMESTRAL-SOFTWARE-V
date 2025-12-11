// Models/Articulo.cs

namespace EcommerceApi.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool PagaItbms { get; set; }

        // Relaciones (opcional, si usas ORM)
        public ICollection<ArticuloCategoria> ArticuloCategorias { get; set; }
        public ICollection<Foto> Fotos { get; set; }
    }
}