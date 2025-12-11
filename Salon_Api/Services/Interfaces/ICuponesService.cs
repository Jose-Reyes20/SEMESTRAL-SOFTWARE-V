using Salon_Api.Modelo;
using Salon_Api.DTO;

namespace Salon_Api.Services.Interfaces
{
    public interface ICuponesService
    {
        Task<CuponDto?> GetCuponPorCodigo(string codigo);
        Task<CuponDto> CrearCupon(Cupon cupon);
        Task<CuponDto?> ActualizarCupon(int id, Cupon cupon);
    }
}
