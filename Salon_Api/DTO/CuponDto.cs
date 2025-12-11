namespace Salon_Api.DTO
{
    public class CuponDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public decimal Descuento { get; set; }
        public bool Estado { get; set; }
    }
}
