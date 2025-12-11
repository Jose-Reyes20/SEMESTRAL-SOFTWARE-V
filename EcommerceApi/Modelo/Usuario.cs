// Models/Usuario.cs

namespace EcommerceApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } // El campo 'usuario' de la tabla SQL
        public string Contrasena { get; set; } // Almacenará el hash de la contraseña
        public string Rol { get; set; } // "admin" | "cliente"

        public int? ClienteId { get; set; } // Puede ser NULL si es un usuario administrador

        // Relación: Un usuario pertenece a un cliente (si rol es 'cliente')
        public Cliente Cliente { get; set; }

        // Relaciones con órdenes y facturas (para futuros pasos)
        // public ICollection<Orden> Ordenes { get; set; } 
        // public ICollection<Factura> Facturas { get; set; }
    }
}