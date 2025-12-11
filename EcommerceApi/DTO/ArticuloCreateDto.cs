// DTOs/ArticuloCreateDto.cs

using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.DTOs
{
	public class ArticuloCreateDto
	{
		[Required]
		[MaxLength(255)]
		public string Nombre { get; set; }

		[MaxLength(255)]
		public string Descripcion { get; set; }

		[Required]
		[Range(0.01, 99999.99)]
		public decimal Precio { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		public int Stock { get; set; }

		[Required]
		public bool PagaItbms { get; set; }

		// Datos para las tablas de relación N:M y Fotos
		public List<int>? CategoriasIds { get; set; }
		public List<string>? FotosUrls { get; set; }
	}
}