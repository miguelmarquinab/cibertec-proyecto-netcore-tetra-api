namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class ContratoListResponse
    {
        public int con_id { get; set; }
        public int cli_id { get; set; }
        public string con_numero { get; set; } = string.Empty;
        public DateTime con_fechaInicio { get; set; }
        public DateTime con_fechaFin { get; set; }
        public decimal? con_valorMensual { get; set; }
        public string? con_estado { get; set; }
        public int nro_radios { get; set; }
        public string cli_razonSocial { get; set; } = string.Empty;
    }
}
