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

            // 1. MAPEO DE TABLAS (Singular)
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

            // 2. MAPEO DE COLUMNAS (Para arreglar el error PagaItbms y otros futuros)

            // Articulo
            modelBuilder.Entity<Articulo>()
                .Property(a => a.PagaItbms).HasColumnName("paga_itbms");

            // Categoria
            modelBuilder.Entity<Categoria>()
                .Property(c => c.CategoriaPadreId).HasColumnName("categoria_padre_id");

            // Usuario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.ClienteId).HasColumnName("cliente_id");

            // Orden
            modelBuilder.Entity<Orden>()
                .Property(o => o.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Orden>()
                .Property(o => o.CuponId).HasColumnName("cupon_id");
            modelBuilder.Entity<Orden>()
                .Property(o => o.Estado).HasConversion<string>(); // Para que el string funcione como enum si es necesario

            // OrdenDetalle
            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.OrdenId).HasColumnName("orden_id");
            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.ArticuloId).HasColumnName("articulo_id");
            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.PrecioUnitario).HasColumnName("precio_unitario");
            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.PrecioFinal).HasColumnName("precio_final");

            // Factura
            modelBuilder.Entity<Factura>()
                .Property(f => f.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Factura>()
                .Property(f => f.CuponId).HasColumnName("cupon_id");

            // FacturaDetalle
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.FacturaId).HasColumnName("factura_id");
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.ArticuloId).HasColumnName("articulo_id");
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.PrecioUnitario).HasColumnName("precio_unitario");
            modelBuilder.Entity<FacturaDetalle>()
                .Property(fd => fd.PrecioFinal).HasColumnName("precio_final");

            // Foto
            modelBuilder.Entity<Foto>()
                .Property(f => f.ArticuloId).HasColumnName("articulo_id");
            modelBuilder.Entity<Foto>()
                .Property(f => f.FotoPrincipal).HasColumnName("foto_principal");

            // ArticuloCategoria
            modelBuilder.Entity<ArticuloCategoria>()
                .Property(ac => ac.IdArticulo).HasColumnName("id_articulo");
            modelBuilder.Entity<ArticuloCategoria>()
                .Property(ac => ac.IdCategoria).HasColumnName("id_categoria");
        }
    }
}