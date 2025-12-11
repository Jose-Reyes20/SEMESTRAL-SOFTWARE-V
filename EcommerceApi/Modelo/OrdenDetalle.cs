// Models/OrdenDetalle.cs

namespace EcommerceApi.Models
{
    public class OrdenDetalle
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }

        public int ArticuloId { get; set; }
        public Articulo Articulo { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioFinal { get; set; }
    }
}