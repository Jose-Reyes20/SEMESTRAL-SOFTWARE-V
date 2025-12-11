namespace Salon_Api.DTO
{
    public class OrdenDto
    {
        public int Id { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public int? CuponId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }
        public decimal Itbms { get; set; }
    }
}
