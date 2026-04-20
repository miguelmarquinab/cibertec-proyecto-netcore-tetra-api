namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class AsignacionRadioRequest
    {
        public int asr_id { get; set; }
        public int dec_id { get; set; }
        public int rad_id { get; set; }
        public DateTime asr_fechaAsignacion { get; set; }
        public string? asr_observacionesAsignacion { get; set; }
        public int usa_id { get; set; }
    }
}
