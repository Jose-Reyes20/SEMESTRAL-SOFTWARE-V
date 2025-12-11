using EcommerceApi.DTOs; // <<-- CORREGIDO
using EcommerceApi.Models; // <<-- CORREGIDO

namespace EcommerceApi.Services.Interfaces // <<-- CORREGIDO
{
    public interface IClientesService
    {
        // Se corrige el nombre de la entidad de retorno de 'Clientes' a 'Cliente'
        Task<IEnumerable<Cliente>> ObtenerClientes();
        Task<Cliente?> ObtenerClientePorId(int id);
        Task<Cliente> CrearCliente(ClienteCreateDto dto);
        Task<Cliente?> ActualizarCliente(int id, ClienteCreateDto dto);
        Task<bool> EliminarCliente(int id);
    }
}