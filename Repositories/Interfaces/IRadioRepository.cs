using ApiSsistemaGestionInventarioRadiosTetra.Models;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces
{
    public interface IRadioRepository
    {
        Task<IEnumerable<RadioListResponse>> ListarAsync(string? texto);
        Task<RadioDetailResponse?> ObtenerAsync(int id);
        Task<int> GuardarAsync(RadioRequest request);
        Task<bool> EliminarAsync(int id);
        Task<IEnumerable<ComboItem>> ListarModelosAsync();
        Task<IEnumerable<ComboItem>> ListarEstadosAsync();
    }
}
