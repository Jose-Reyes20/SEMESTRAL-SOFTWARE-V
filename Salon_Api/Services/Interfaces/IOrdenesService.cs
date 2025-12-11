using Salon_Api.Modelo;
using Salon_Api.DTO;

namespace Salon_Api.Services.Interfaces
{
    public interface IOrdenesService
    {
        Task<IEnumerable<OrdenDto>> GetOrdenes();
        Task<IEnumerable<OrdenDto>> GetOrdenesPorUsuario(int usuarioId);
        Task<OrdenDto> CrearOrden(Orden orden);
        Task<OrdenDto?> GetOrdenPorId(int id);
        Task<bool> ActualizarEstado(int id, string nuevoEstado);
    }
}
