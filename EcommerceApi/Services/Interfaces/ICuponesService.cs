using EcommerceApi.DTOs; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO

namespace EcommerceApi.Services.Interfaces // <<-- CORREGIDO
{
    public interface ICuponesService
    {
        Task<CuponDto?> GetCuponPorCodigo(string codigo);
        // Nota: Idealmente, CrearCupon y ActualizarCupon deberían usar DTOs de entrada, no el modelo Cupon.
        Task<CuponDto> CrearCupon(Cupon cupon);
        Task<CuponDto?> ActualizarCupon(int id, Cupon cupon);
    }
}