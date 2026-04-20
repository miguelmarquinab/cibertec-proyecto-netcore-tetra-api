using ApiSsistemaGestionInventarioRadiosTetra.Models.Dto;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<UsuarioLoginDto?> LoginAsync(string nombre, string clave);
    }
}
