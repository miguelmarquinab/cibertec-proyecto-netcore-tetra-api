namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class RadioRequest
    {
        public int rad_id { get; set; }
        public int mod_id { get; set; }
        public int esr_id { get; set; }
        public string serie { get; set; } = "";
        public DateTime? fecha_ingreso { get; set; }
        public bool rad_activo { get; set; }
    }
}
