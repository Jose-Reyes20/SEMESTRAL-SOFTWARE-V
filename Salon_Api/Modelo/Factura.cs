using EcommerceApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salon_Api.Modelo
{
    [Table("factura")]
    public class Factura
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cupon_id")]
        public int? CuponId { get; set; }

        [ForeignKey("CuponId")]
        public Cupon? Cupon { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("descuento")]
        public decimal Descuento { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("itbms")]
        public decimal Itbms { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        
    }
}
