using Microsoft.EntityFrameworkCore;
using EcommerceApi.Models;

namespace EcommerceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets (Tablas en C#)
        public DbSet<Articulo> Articulos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Cupon> Cupones { get; set; } = null!;
        public DbSet<Orden> Ordenes { get; set; } = null!;
        public DbSet<Factura> Facturas { get; set; } = null!;
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; } = null!;
        public DbSet<FacturaDetalle> FacturaDetalles { get; set; } = null!;
        public DbSet<ArticuloCategoria> ArticuloCategorias { get; set; } = null!;
        public DbSet<Foto> Fotos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =================================================================
            // 1. MAPEO DE NOMBRES DE TABLAS (Singular)
            // =================================================================
            modelBuilder.Entity<Articulo>().ToTable("articulo");
            modelBuilder.Entity<Categoria>().ToTable("categoria");
            modelBuilder.Entity<Cliente>().ToTable("cliente");
            modelBuilder.Entity<Usuario>().ToTable("usuario");
            modelBuilder.Entity<Orden>().ToTable("orden");
            modelBuilder.Entity<OrdenDetalle>().ToTable("orden_detalle");
            modelBuilder.Entity<Factura>().ToTable("factura");
            modelBuilder.Entity<FacturaDetalle>().ToTable("factura_detalle");
            modelBuilder.Entity<Cupon>().ToTable("cupon");
            modelBuilder.Entity<ArticuloCategoria>().ToTable("articulo_categoria");
            modelBuilder.Entity<Foto>().ToTable("foto");

            // =================================================================
            // 2. MAPEO DE COLUMNAS (Correcciones específicas)
            // =================================================================

            // --- USUARIO (Corrección de Username y Contrasena) ---
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Mapea la propiedad C# 'Username' a la columna SQL 'usuario'
                entity.Property(u => u.Username).HasColumnName("usuario");

                // Mapea la propiedad C# 'Contrasena' a la columna SQL 'contrasena'
                entity.Property(u => u.Contrasena).HasColumnName("contrasena");

                entity.Property(u => u.ClienteId).HasColumnName("cliente_id");
                entity.Property(u => u.Rol).HasColumnName("rol");

                // Si tienes fechas en tu modelo, asegúrate de mapearlas también si difieren
                // entity.Property(u => u.CreatedAt).HasColumnName("created_at");
            });

            // --- ARTICULO ---
            modelBuilder.Entity<Articulo>()
                .Property(a => a.PagaItbms).HasColumnName("paga_itbms");

            // --- CATEGORIA ---
            modelBuilder.Entity<Categoria>()
                .Property(c => c.CategoriaPadreId).HasColumnName("categoria_padre_id");

            // --- ORDEN ---
            modelBuilder.Entity<Orden>(entity =>
            {
                entity.Property(o => o.UsuarioId).HasColumnName("usuario_id");
                entity.Property(o => o.CuponId).HasColumnName("cupon_id");
                // Conversión de Enum a String para el estado
                entity.Property(o => o.Estado).HasConversion<string>();
            });

            // --- ORDEN DETALLE ---
            modelBuilder.Entity<OrdenDetalle>(entity =>
            {
                entity.Property(od => od.OrdenId).HasColumnName("orden_id");
                entity.Property(od => od.ArticuloId).HasColumnName("articulo_id");
                entity.Property(od => od.PrecioUnitario).HasColumnName("precio_unitario");
                entity.Property(od => od.PrecioFinal).HasColumnName("precio_final");
            });

            // --- FACTURA ---
            modelBuilder.Entity<Factura>(entity =>
            {
                entity.Property(f => f.UsuarioId).HasColumnName("usuario_id");
                entity.Property(f => f.CuponId).HasColumnName("cupon_id");
            });

            // --- FACTURA DETALLE ---
            modelBuilder.Entity<FacturaDetalle>(entity =>
            {
                entity.Property(fd => fd.FacturaId).HasColumnName("factura_id");
                entity.Property(fd => fd.ArticuloId).HasColumnName("articulo_id");
                entity.Property(fd => fd.PrecioUnitario).HasColumnName("precio_unitario");
                entity.Property(fd => fd.PrecioFinal).HasColumnName("precio_final");
            });

            // --- FOTO ---
            modelBuilder.Entity<Foto>(entity =>
            {
                entity.Property(f => f.ArticuloId).HasColumnName("articulo_id");
                entity.Property(f => f.FotoPrincipal).HasColumnName("foto_principal");
            });

            // --- ARTICULO CATEGORIA ---
            modelBuilder.Entity<ArticuloCategoria>(entity =>
            {
                entity.Property(ac => ac.IdArticulo).HasColumnName("id_articulo");
                entity.Property(ac => ac.IdCategoria).HasColumnName("id_categoria");
            });
        }
    }
}