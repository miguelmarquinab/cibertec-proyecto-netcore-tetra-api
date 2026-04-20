namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class RadioDetailResponse
    {
        public int radio_id { get; set; }
        public int? mod_id { get; set; }
        public int? esr_id { get; set; }
        public string serie { get; set; } = "";
        public string fecha_ingreso { get; set; } = "";
        public bool? rad_activo { get; set; }
        public string mod_codigo { get; set; } = "";
        public string modelo { get; set; } = "";
        public string estado { get; set; } = "";
    }
}
