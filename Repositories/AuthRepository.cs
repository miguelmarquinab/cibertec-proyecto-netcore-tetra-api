using ApiSsistemaGestionInventarioRadiosTetra.Models.Dto;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("cn1")!;
        }

        public async Task<UsuarioLoginDto?> LoginAsync(string nombre, string clave)
        {
            UsuarioLoginDto? item = null;

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_usuario_login", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_nombre", nombre);
            cmd.Parameters.AddWithValue("@p_clave", clave);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                item = new UsuarioLoginDto
                {
                    usa_id = Convert.ToInt32(dr["usa_id"]),
                    nombre = dr["nombre"]?.ToString() ?? string.Empty,
                    usa_nombres = dr["usa_nombres"]?.ToString() ?? string.Empty,
                    usa_apellidopaterno = dr["usa_apellidopaterno"]?.ToString() ?? string.Empty,
                    usa_apellidomaterno = dr["usa_apellidomaterno"]?.ToString() ?? string.Empty,
                    usa_estado = dr["usa_estado"]?.ToString(),
                    usa_activo = dr["usa_activo"]?.ToString(),
                    rol = dr["rol"] == DBNull.Value ? null : Convert.ToInt32(dr["rol"]),
                    rol_nombre = dr["rol_nombre"]?.ToString()
                };
            }
            return item;
        }
    }
}
