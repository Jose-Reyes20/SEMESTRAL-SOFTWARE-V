namespace EcommerceApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string Rol { get; set; } = null!;

        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}