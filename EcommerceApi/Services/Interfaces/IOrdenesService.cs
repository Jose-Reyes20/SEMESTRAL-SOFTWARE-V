using EcommerceApi.Models; // <<-- CORREGIDO
using EcommerceApi.DTOs; // <<-- CORREGIDO

namespace EcommerceApi.Services.Interfaces // <<-- CORREGIDO
{
    public interface IOrdenesService
    {
        Task<IEnumerable<OrdenDto>> GetOrdenes();
        Task<IEnumerable<OrdenDto>> GetOrdenesPorUsuario(int usuarioId);
        // Nota: Idealmente, CrearOrden debería usar un DTO (OrdenCreateDto) de entrada.
        Task<OrdenDto> CrearOrden(Orden orden);
        Task<OrdenDto?> GetOrdenPorId(int id);
        Task<bool> ActualizarEstado(int id, string nuevoEstado);
    }
}