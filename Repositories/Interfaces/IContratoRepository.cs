using ApiSsistemaGestionInventarioRadiosTetra.Models;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces
{
    public interface IContratoRepository
    {
        Task<IEnumerable<ContratoListResponse>> ListarAsync(int clienteId, string? estado, string? fechaInicio, string? fechaFin);
        Task<ContratoResponse?> ObtenerAsync(int conId);
        Task<int> GuardarAsync(ContratoRequest request);
        Task<bool> EliminarLogicoAsync(int conId);
    }
}
