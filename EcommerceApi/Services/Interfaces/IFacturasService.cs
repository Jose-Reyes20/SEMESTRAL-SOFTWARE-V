using EcommerceApi.Models; // <<-- CORREGIDO (Necesario para referenciar Factura si lo requiere el DTO o métodos futuros)
using EcommerceApi.DTOs; // <<-- CORREGIDO

namespace EcommerceApi.Services.Interfaces // <<-- CORREGIDO
{
    public interface IFacturasService
    {
        Task<IEnumerable<FacturaDto>> GetFacturas();
        Task<FacturaDto?> GetFacturaPorId(int id);
    }
}