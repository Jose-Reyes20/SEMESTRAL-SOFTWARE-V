// Models/Foto.cs

namespace EcommerceApi.Models
{
    public class Foto
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public Articulo Articulo { get; set; }

        // Asumiendo que guardamos solo la URL principal aquí
        public string FotoPrincipal { get; set; } = null!;
        public string? Foto2 { get; set; }
        public string? Foto3 { get; set; }
        public string? Foto4 { get; set; }
    }
}