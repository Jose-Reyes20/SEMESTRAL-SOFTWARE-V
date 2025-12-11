// Models/FacturaDetalle.cs

namespace EcommerceApi.Models
{
    public class FacturaDetalle
    {
        public int Id { get; set; }

        public int FacturaId { get; set; }
        public Factura Factura { get; set; } = null!; // Propiedad de Navegación

        public int ArticuloId { get; set; }
        public Articulo Articulo { get; set; } = null!; // Propiedad de Navegación

        public decimal PrecioUnitario { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}