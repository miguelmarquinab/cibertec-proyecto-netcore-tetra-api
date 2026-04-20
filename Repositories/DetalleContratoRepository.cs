using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories
{
    public class DetalleContratoRepository : IDetalleContratoRepository
    {
        private readonly string _connectionString;

        public DetalleContratoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("cn")!;
        }

        public async Task<IEnumerable<DetalleContratoResponse>> ListarPorContratoAsync(int conId)
        {
            var lista = new List<DetalleContratoResponse>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_detalleContrato_listar_por_contrato", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_con_id", conId);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new DetalleContratoResponse
                {
                    dec_id = Convert.ToInt32(dr["dec_id"]),
                    con_id = Convert.ToInt32(dr["con_id"]),
                    mod_id = Convert.ToInt32(dr["mod_id"]),
                    dec_cantidad = Convert.ToInt32(dr["dec_cantidad"]),
                    dec_precioUnitario = Convert.ToDecimal(dr["dec_precioUnitario"]),
                    dec_subtotal = Convert.ToDecimal(dr["dec_subtotal"]),
                    dec_filaEliminada = dr["dec_filaEliminada"] == DBNull.Value ? null : Convert.ToBoolean(dr["dec_filaEliminada"]),
                    usa_id = dr["usa_id"] == DBNull.Value ? null : Convert.ToInt32(dr["usa_id"]),
                    mod_codigo = dr["mod_codigo"]?.ToString(),
                    mod_descripcion = dr["mod_descripcion"]?.ToString(),
                    con_numero = dr["con_numero"]?.ToString()
                });
            }

            return lista;
        }

        public async Task<DetalleContratoResponse?> ObtenerAsync(int decId)
        {
            DetalleContratoResponse? item = null;

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_detalleContrato_obtener", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_dec_id", decId);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                item = new DetalleContratoResponse
                {
                    dec_id = Convert.ToInt32(dr["dec_id"]),
                    con_id = Convert.ToInt32(dr["con_id"]),
                    mod_id = Convert.ToInt32(dr["mod_id"]),
                    dec_cantidad = Convert.ToInt32(dr["dec_cantidad"]),
                    dec_precioUnitario = Convert.ToDecimal(dr["dec_precioUnitario"]),
                    dec_subtotal = Convert.ToDecimal(dr["dec_subtotal"]),
                    dec_filaEliminada = dr["dec_filaEliminada"] == DBNull.Value ? null : Convert.ToBoolean(dr["dec_filaEliminada"]),
                    usa_id = dr["usa_id"] == DBNull.Value ? null : Convert.ToInt32(dr["usa_id"]),
                    mod_codigo = dr["mod_codigo"]?.ToString(),
                    mod_descripcion = dr["mod_descripcion"]?.ToString(),
                    con_numero = dr["con_numero"]?.ToString()
                };
            }

            return item;
        }

        public async Task<int> GuardarAsync(DetalleContratoRequest request)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_detalleContrato_guardar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_dec_id", request.dec_id);
            cmd.Parameters.AddWithValue("@p_con_id", request.con_id);
            cmd.Parameters.AddWithValue("@p_mod_id", request.mod_id);
            cmd.Parameters.AddWithValue("@p_dec_cantidad", request.dec_cantidad);
            cmd.Parameters.AddWithValue("@p_dec_precioUnitario", request.dec_precioUnitario);
            cmd.Parameters.AddWithValue("@p_usa_id", request.usa_id);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            return result == null ? 0 : Convert.ToInt32(result);
        }

        public async Task<bool> EliminarLogicoAsync(int decId)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_detalleContrato_eliminar_logico", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_dec_id", decId);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            int filas = result == null ? 0 : Convert.ToInt32(result);
            return filas > 0;
        }
    }
}
