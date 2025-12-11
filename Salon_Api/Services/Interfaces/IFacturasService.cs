using Salon_Api.Modelo;
using Salon_Api.DTO;

namespace Salon_Api.Services.Interfaces
{
    public interface IFacturasService
    {
        Task<IEnumerable<FacturaDto>> GetFacturas();
        Task<FacturaDto?> GetFacturaPorId(int id);
    }
}
