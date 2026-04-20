using ApiSsistemaGestionInventarioRadiosTetra.Models;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces
{
    public interface IAsignacionRadioRepository
    {
        Task<IEnumerable<AsignacionRadioResponse>> ListarPorDetalleAsync(int decId);
        Task<AsignacionRadioResponse?> ObtenerAsync(int asrId);
        Task<IEnumerable<RadioDisponibleResponse>> ListarDisponiblesPorModeloAsync(int modId);
        Task<int> GuardarAsync(AsignacionRadioRequest request);
        Task<bool> DevolverAsync(int asrId, AsignacionRadioDevolucionRequest request);
        Task<bool> EliminarLogicoAsync(int asrId);
    }
}
