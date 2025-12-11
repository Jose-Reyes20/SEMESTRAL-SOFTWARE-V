namespace EcommerceApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        // El "= null!;" le dice al compilador: "Tranquilo, esto tendrá datos de la base de datos".
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;

        // Relación (Puede ser nula si no se ha cargado)
        public Usuario? Usuario { get; set; }
    }
}