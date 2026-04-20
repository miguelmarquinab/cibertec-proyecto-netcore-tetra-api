using ApiSsistemaGestionInventarioRadiosTetra.Models.Dto;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiSsistemaGestionInventarioRadiosTetra.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UsuarioLoginDto usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.usa_id.ToString()),
                new Claim(ClaimTypes.Name, usuario.nombre),
                new Claim("usa_id", usuario.usa_id.ToString()),
                new Claim("nombre", usuario.nombre),
                new Claim("nombres", usuario.usa_nombres ?? string.Empty),
                new Claim("apellidopaterno", usuario.usa_apellidopaterno ?? string.Empty),
                new Claim("apellidomaterno", usuario.usa_apellidomaterno ?? string.Empty)
            };

            if (!string.IsNullOrWhiteSpace(usuario.rol_nombre))
            {
                claims.Add(new Claim(ClaimTypes.Role, usuario.rol_nombre));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpireMinutes"])
                ),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
