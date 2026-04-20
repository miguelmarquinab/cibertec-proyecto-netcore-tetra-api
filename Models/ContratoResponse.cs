namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class ContratoResponse
    {
        public int con_id { get; set; }
        public int cli_id { get; set; }
        public string con_numero { get; set; } = string.Empty;
        public DateTime con_fechaInicio { get; set; }
        public DateTime con_fechaFin { get; set; }
        public string? con_estado { get; set; }
        public string? con_tipoContrato { get; set; }
        public decimal? con_valorTotal { get; set; }
        public decimal? con_valorMensual { get; set; }
        public string? con_observaciones { get; set; }
        public bool? con_filaEliminada { get; set; }
        public int? usa_id { get; set; }
        public string? cli_codigo { get; set; }
        public string? cli_razonSocial { get; set; }
    }
}
