using ApiSsistemaGestionInventarioRadiosTetra.Models;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces
{
    public interface IDetalleContratoRepository
    {
        Task<IEnumerable<DetalleContratoResponse>> ListarPorContratoAsync(int conId);
        Task<DetalleContratoResponse?> ObtenerAsync(int decId);
        Task<int> GuardarAsync(DetalleContratoRequest request);
        Task<bool> EliminarLogicoAsync(int decId);
    }
}
