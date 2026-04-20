using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly string _connectionString;

        public ContratoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("cn")!;
        }

        public async Task<IEnumerable<ContratoListResponse>> ListarAsync(
            int clienteId,
            string? estado,
            string? fechaInicio,
            string? fechaFin)
        {
            var lista = new List<ContratoListResponse>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_Buscar_Contratos", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@cliente_id", clienteId);
            cmd.Parameters.AddWithValue("@estado", (object?)estado ?? string.Empty);
            cmd.Parameters.AddWithValue("@fecha_inicio", (object?)fechaInicio ?? string.Empty);
            cmd.Parameters.AddWithValue("@fecha_fin", (object?)fechaFin ?? string.Empty);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new ContratoListResponse
                {
                    con_id = Convert.ToInt32(dr["con_id"]),
                    cli_id = Convert.ToInt32(dr["cli_id"]),
                    con_numero = dr["con_numero"]?.ToString() ?? string.Empty,
                    con_fechaInicio = Convert.ToDateTime(dr["con_fechaInicio"]),
                    con_fechaFin = Convert.ToDateTime(dr["con_fechaFin"]),
                    con_valorMensual = dr["con_valorMensual"] == DBNull.Value ? null : Convert.ToDecimal(dr["con_valorMensual"]),
                    con_estado = dr["con_estado"]?.ToString(),
                    nro_radios = Convert.ToInt32(dr["nro_radios"]),
                    cli_razonSocial = dr["cli_razonSocial"]?.ToString() ?? string.Empty
                });
            }
            return lista;
        }

        public async Task<ContratoResponse?> ObtenerAsync(int conId)
        {
            ContratoResponse? item = null;
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_contrato_obtener", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_con_id", conId);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                item = new ContratoResponse
                {
                    con_id = Convert.ToInt32(dr["con_id"]),
                    cli_id = Convert.ToInt32(dr["cli_id"]),
                    con_numero = dr["con_numero"]?.ToString() ?? string.Empty,
                    con_fechaInicio = Convert.ToDateTime(dr["con_fechaInicio"]),
                    con_fechaFin = Convert.ToDateTime(dr["con_fechaFin"]),
                    con_estado = dr["con_estado"]?.ToString(),
                    con_tipoContrato = dr["con_tipoContrato"]?.ToString(),
                    con_valorTotal = dr["con_valorTotal"] == DBNull.Value ? null : Convert.ToDecimal(dr["con_valorTotal"]),
                    con_valorMensual = dr["con_valorMensual"] == DBNull.Value ? null : Convert.ToDecimal(dr["con_valorMensual"]),
                    con_observaciones = dr["con_observaciones"]?.ToString(),
                    con_filaEliminada = dr["con_filaEliminada"] == DBNull.Value ? null : Convert.ToBoolean(dr["con_filaEliminada"]),
                    usa_id = dr["usa_id"] == DBNull.Value ? null : Convert.ToInt32(dr["usa_id"]),
                    cli_codigo = dr["cli_codigo"]?.ToString(),
                    cli_razonSocial = dr["cli_razonSocial"]?.ToString()
                };
            }
            return item;
        }

        public async Task<int> GuardarAsync(ContratoRequest request)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_contrato_guardar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_con_id", request.con_id);
            cmd.Parameters.AddWithValue("@p_cli_id", request.cli_id);
            cmd.Parameters.AddWithValue("@p_con_numero", request.con_numero);
            cmd.Parameters.AddWithValue("@p_con_fechaInicio", request.con_fechaInicio.Date);
            cmd.Parameters.AddWithValue("@p_con_fechaFin", request.con_fechaFin.Date);
            cmd.Parameters.AddWithValue("@p_con_estado", (object?)request.con_estado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_con_tipoContrato", (object?)request.con_tipoContrato ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_con_valorTotal", (object?)request.con_valorTotal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_con_valorMensual", (object?)request.con_valorMensual ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_con_observaciones", (object?)request.con_observaciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_usa_id", request.usa_id);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            return result == null ? 0 : Convert.ToInt32(result);
        }

        public async Task<bool> EliminarLogicoAsync(int conId)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_contrato_eliminar_logico", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_con_id", conId);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            int filas = result == null ? 0 : Convert.ToInt32(result);
            return filas > 0;
        }
    }
}
