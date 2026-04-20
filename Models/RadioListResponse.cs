namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class RadioListResponse
    {
        public int rad_id { get; set; }
        public string mod_codigo { get; set; } = "";
        public string modelo { get; set; } = "";
        public string estado { get; set; } = "";
        public string serie { get; set; } = "";
        public DateTime? fecha_ingreso { get; set; }
        public bool? rad_activo { get; set; }
    }
}
