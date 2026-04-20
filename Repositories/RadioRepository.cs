using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories
{
    public class RadioRepository : IRadioRepository
    {
        private readonly string _connectionString;

        public RadioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("cn1")!;
        }

        public async Task<IEnumerable<RadioListResponse>> ListarAsync(string? texto)
        {
            var lista = new List<RadioListResponse>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_Buscar_Radio", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_texto", (object?)texto ?? DBNull.Value);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new RadioListResponse
                {
                    rad_id = Convert.ToInt32(dr["rad_id"]),
                    mod_codigo = dr["mod_codigo"]?.ToString() ?? "",
                    modelo = dr["modelo"]?.ToString() ?? "",
                    estado = dr["estado"]?.ToString() ?? "",
                    serie = dr["serie"]?.ToString() ?? "",
                    fecha_ingreso = dr["fecha_ingreso"] == DBNull.Value ? null : Convert.ToDateTime(dr["fecha_ingreso"]),
                    rad_activo = dr["rad_activo"] == DBNull.Value ? null : Convert.ToBoolean(dr["rad_activo"])
                });
            }
            return lista;
        }

        public async Task<RadioDetailResponse?> ObtenerAsync(int id)
        {
            RadioDetailResponse? item = null;

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_radio_obtener", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_rad_id", id);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                item = new RadioDetailResponse
                {
                    radio_id = Convert.ToInt32(dr["radio_id"]),
                    mod_id = dr["mod_id"] == DBNull.Value ? null : Convert.ToInt32(dr["mod_id"]),
                    esr_id = dr["esr_id"] == DBNull.Value ? null : Convert.ToInt32(dr["esr_id"]),
                    serie = dr["serie"]?.ToString() ?? "",
                    fecha_ingreso = dr["fecha_ingreso"]?.ToString() ?? "",
                    rad_activo = dr["rad_activo"] == DBNull.Value ? null : Convert.ToBoolean(dr["rad_activo"]),
                    mod_codigo = dr["mod_codigo"]?.ToString() ?? "",
                    modelo = dr["modelo"]?.ToString() ?? "",
                    estado = dr["estado"]?.ToString() ?? ""
                };
            }
            return item;
        }

        public async Task<int> GuardarAsync(RadioRequest request)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_radio_guardar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_rad_id", request.rad_id);
            cmd.Parameters.AddWithValue("@p_mod_id", request.mod_id);
            cmd.Parameters.AddWithValue("@p_esr_id", request.esr_id);
            cmd.Parameters.AddWithValue("@p_serie", request.serie);
            cmd.Parameters.AddWithValue("@p_fecha_ingreso", (object?)request.fecha_ingreso ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_activo", request.rad_activo);

            await cn.OpenAsync();

            object? result = await cmd.ExecuteScalarAsync();
            return result == null ? 0 : Convert.ToInt32(result);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_radio_eliminar", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_rad_id", id);

            await cn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<IEnumerable<ComboItem>> ListarModelosAsync()
        {
            var lista = new List<ComboItem>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_radio_modelos", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new ComboItem
                {
                    id = Convert.ToInt32(dr["mod_id"]),
                    codigo = dr["mod_codigo"]?.ToString() ?? "",
                    descripcion = dr["mod_descripcion"]?.ToString() ?? ""
                });
            }
            return lista;
        }

        public async Task<IEnumerable<ComboItem>> ListarEstadosAsync()
        {
            var lista = new List<ComboItem>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_radio_estados", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new ComboItem
                {
                    id = Convert.ToInt32(dr["esr_id"]),
                    codigo = "",
                    descripcion = dr["esr_descripcion"]?.ToString() ?? ""
                });
            }

            return lista;
        }
    }
}
