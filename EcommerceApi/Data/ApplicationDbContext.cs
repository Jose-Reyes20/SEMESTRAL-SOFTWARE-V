// Data/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models; // Usar el namespace unificado

namespace EcommerceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tablas principales
        public DbSet<Articulo> Articulos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        // Tablas de Transacciones y Cupones
        public DbSet<Cupon> Cupones { get; set; } = null!;
        public DbSet<Orden> Ordenes { get; set; } = null!;
        public DbSet<Factura> Facturas { get; set; } = null!;

        // Tablas de Relación y Detalles (Faltantes)
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; } = null!;
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; } = null!;
        public DbSet<ArticuloCategoria> ArticuloCategorias { get; set; } = null!;
        public DbSet<Foto> Fotos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de clave compuesta (ejemplo para ArticuloCategoria si no tuviera un ID)
            // modelBuilder.Entity<ArticuloCategoria>().HasKey(ac => new { ac.IdArticulo, ac.IdCategoria });

            // Configuraciones específicas de las tablas (ej: restricciones CHECK de tu SQL)
            modelBuilder.Entity<Orden>()
                .Property(o => o.Estado)
                .HasConversion<string>(); // Asegura que el enum/string del estado se guarde como string

            // Mapear el nombre real de la tabla si es diferente al nombre del DbSet
            // modelBuilder.Entity<Cliente>().ToTable("cliente"); 
        }
    }
}