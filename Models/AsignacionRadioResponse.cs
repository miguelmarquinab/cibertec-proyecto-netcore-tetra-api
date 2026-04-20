namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class AsignacionRadioResponse
    {
        public int asr_id { get; set; }
        public int dec_id { get; set; }
        public int rad_id { get; set; }
        public DateTime asr_fechaAsignacion { get; set; }
        public DateTime? asr_fechaDevolucion { get; set; }
        public string? asr_estado { get; set; }
        public string? asr_observacionesAsignacion { get; set; }
        public string? asr_observacionesDevolucion { get; set; }
        public bool? asr_filaEliminada { get; set; }
        public int? usa_id { get; set; }
        public string? serie { get; set; }
        public string? mod_codigo { get; set; }
        public string? mod_descripcion { get; set; }
        public string? esr_descripcion { get; set; }
    }
}
