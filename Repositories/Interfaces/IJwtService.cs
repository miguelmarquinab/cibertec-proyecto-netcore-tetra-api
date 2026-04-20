using ApiSsistemaGestionInventarioRadiosTetra.Models.Dto;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UsuarioLoginDto usuario);
    }
}
