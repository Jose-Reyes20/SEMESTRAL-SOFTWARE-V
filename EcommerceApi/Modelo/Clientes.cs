// Models/Cliente.cs

namespace EcommerceApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; } // Único

        // Relación: Un cliente puede tener un usuario
        public Usuario Usuario { get; set; }
    }
}