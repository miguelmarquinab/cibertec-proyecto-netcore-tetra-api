using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiSsistemaGestionInventarioRadiosTetra.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _authRepository.LoginAsync(request.nombre, request.clave);

            if (usuario == null)
            {
                return Unauthorized(new
                {
                    message = "Usuario o clave incorrectos"
                });
            }

            var token = _jwtService.GenerateToken(usuario);

            var response = new LoginResponse
            {
                usa_id = usuario.usa_id,
                nombre = usuario.nombre,
                usa_nombres = usuario.usa_nombres,
                usa_apellidopaterno = usuario.usa_apellidopaterno,
                usa_apellidomaterno = usuario.usa_apellidomaterno,
                usa_estado = usuario.usa_estado,
                usa_activo = usuario.usa_activo,
                rol = usuario.rol,
                rol_nombre = usuario.rol_nombre,
                token = token
            };

            return Ok(response);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                usa_id = User.FindFirst("usa_id")?.Value,
                nombre = User.FindFirst("nombre")?.Value,
                role = User.Claims.Where(c => c.Type.EndsWith("/role") || c.Type == "role")
                                  .Select(c => c.Value)
                                  .FirstOrDefault()
            });
        }
    }
}
