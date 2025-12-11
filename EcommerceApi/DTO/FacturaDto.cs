// FacturaDto.cs (Corregido)

namespace EcommerceApi.DTOs // <<-- CORREGIDO
{
    public class FacturaDto
    {
        public int Id { get; set; }
        public int? CuponId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Itbms { get; set; }
        public int UsuarioId { get; set; }
    }
}