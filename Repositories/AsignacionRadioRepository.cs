using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiSsistemaGestionInventarioRadiosTetra.Repositories
{
    public class AsignacionRadioRepository : IAsignacionRadioRepository
    {
        private readonly string _connectionString;

        public AsignacionRadioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("cn")!;
        }

        public async Task<IEnumerable<AsignacionRadioResponse>> ListarPorDetalleAsync(int decId)
        {
            var lista = new List<AsignacionRadioResponse>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_asignacionRadio_listar_por_detalle", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_dec_id", decId);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new AsignacionRadioResponse
                {
                    asr_id = Convert.ToInt32(dr["asr_id"]),
                    dec_id = Convert.ToInt32(dr["dec_id"]),
                    rad_id = Convert.ToInt32(dr["rad_id"]),
                    asr_fechaAsignacion = Convert.ToDateTime(dr["asr_fechaAsignacion"]),
                    asr_fechaDevolucion = dr["asr_fechaDevolucion"] == DBNull.Value ? null : Convert.ToDateTime(dr["asr_fechaDevolucion"]),
                    asr_estado = dr["asr_estado"]?.ToString(),
                    asr_observacionesAsignacion = dr["asr_observacionesAsignacion"]?.ToString(),
                    asr_observacionesDevolucion = dr["asr_observacionesDevolucion"]?.ToString(),
                    asr_filaEliminada = dr["asr_filaEliminada"] == DBNull.Value ? null : Convert.ToBoolean(dr["asr_filaEliminada"]),
                    usa_id = dr["usa_id"] == DBNull.Value ? null : Convert.ToInt32(dr["usa_id"]),
                    serie = dr["serie"]?.ToString(),
                    mod_codigo = dr["mod_codigo"]?.ToString(),
                    mod_descripcion = dr["mod_descripcion"]?.ToString(),
                    esr_descripcion = dr["esr_descripcion"]?.ToString()
                });
            }

            return lista;
        }

        public async Task<AsignacionRadioResponse?> ObtenerAsync(int asrId)
        {
            AsignacionRadioResponse? item = null;

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_asignacionRadio_obtener", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_asr_id", asrId);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            if (await dr.ReadAsync())
            {
                item = new AsignacionRadioResponse
                {
                    asr_id = Convert.ToInt32(dr["asr_id"]),
                    dec_id = Convert.ToInt32(dr["dec_id"]),
                    rad_id = Convert.ToInt32(dr["rad_id"]),
                    asr_fechaAsignacion = Convert.ToDateTime(dr["asr_fechaAsignacion"]),
                    asr_fechaDevolucion = dr["asr_fechaDevolucion"] == DBNull.Value ? null : Convert.ToDateTime(dr["asr_fechaDevolucion"]),
                    asr_estado = dr["asr_estado"]?.ToString(),
                    asr_observacionesAsignacion = dr["asr_observacionesAsignacion"]?.ToString(),
                    asr_observacionesDevolucion = dr["asr_observacionesDevolucion"]?.ToString(),
                    asr_filaEliminada = dr["asr_filaEliminada"] == DBNull.Value ? null : Convert.ToBoolean(dr["asr_filaEliminada"]),
                    usa_id = dr["usa_id"] == DBNull.Value ? null : Convert.ToInt32(dr["usa_id"]),
                    serie = dr["serie"]?.ToString(),
                    mod_codigo = dr["mod_codigo"]?.ToString(),
                    mod_descripcion = dr["mod_descripcion"]?.ToString(),
                    esr_descripcion = dr["esr_descripcion"]?.ToString()
                };
            }

            return item;
        }

        public async Task<IEnumerable<RadioDisponibleResponse>> ListarDisponiblesPorModeloAsync(int modId)
        {
            var lista = new List<RadioDisponibleResponse>();

            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_radio_disponibles_por_modelo", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_mod_id", modId);

            await cn.OpenAsync();
            using SqlDataReader dr = await cmd.ExecuteReaderAsync();

            while (await dr.ReadAsync())
            {
                lista.Add(new RadioDisponibleResponse
                {
                    rad_id = Convert.ToInt32(dr["rad_id"]),
                    mod_id = dr["mod_id"] == DBNull.Value ? null : Convert.ToInt32(dr["mod_id"]),
                    esr_id = dr["esr_id"] == DBNull.Value ? null : Convert.ToInt32(dr["esr_id"]),
                    serie = dr["serie"]?.ToString(),
                    fecha_ingreso = dr["fecha_ingreso"] == DBNull.Value ? null : Convert.ToDateTime(dr["fecha_ingreso"]),
                    rad_activo = dr["rad_activo"] == DBNull.Value ? null : Convert.ToBoolean(dr["rad_activo"]),
                    mod_codigo = dr["mod_codigo"]?.ToString(),
                    mod_descripcion = dr["mod_descripcion"]?.ToString(),
                    esr_descripcion = dr["esr_descripcion"]?.ToString()
                });
            }

            return lista;
        }

        public async Task<int> GuardarAsync(AsignacionRadioRequest request)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_asignacionRadio_guardar", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_asr_id", request.asr_id);
            cmd.Parameters.AddWithValue("@p_dec_id", request.dec_id);
            cmd.Parameters.AddWithValue("@p_rad_id", request.rad_id);
            cmd.Parameters.AddWithValue("@p_asr_fechaAsignacion", request.asr_fechaAsignacion.Date);
            cmd.Parameters.AddWithValue("@p_asr_observacionesAsignacion", (object?)request.asr_observacionesAsignacion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@p_usa_id", request.usa_id);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            return result == null ? 0 : Convert.ToInt32(result);
        }

        public async Task<bool> DevolverAsync(int asrId, AsignacionRadioDevolucionRequest request)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_asignacionRadio_devolver", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_asr_id", asrId);
            cmd.Parameters.AddWithValue("@p_asr_fechaDevolucion", request.asr_fechaDevolucion.Date);
            cmd.Parameters.AddWithValue("@p_asr_observacionesDevolucion", (object?)request.asr_observacionesDevolucion ?? DBNull.Value);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            int filas = result == null ? 0 : Convert.ToInt32(result);
            return filas > 0;
        }

        public async Task<bool> EliminarLogicoAsync(int asrId)
        {
            using SqlConnection cn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("dbo.usp_asignacionRadio_eliminar_logico", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_asr_id", asrId);

            await cn.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();

            int filas = result == null ? 0 : Convert.ToInt32(result);
            return filas > 0;
        }

    }
}
