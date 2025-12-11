namespace EcommerceApi.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool PagaItbms { get; set; }

        public ICollection<ArticuloCategoria>? ArticuloCategorias { get; set; }
        public ICollection<Foto>? Fotos { get; set; }
    }
}