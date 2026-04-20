namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class DetalleContratoResponse
    {
        public int dec_id { get; set; }
        public int con_id { get; set; }
        public int mod_id { get; set; }
        public int dec_cantidad { get; set; }
        public decimal dec_precioUnitario { get; set; }
        public decimal dec_subtotal { get; set; }
        public bool? dec_filaEliminada { get; set; }
        public int? usa_id { get; set; }
        public string? mod_codigo { get; set; }
        public string? mod_descripcion { get; set; }
        public string? con_numero { get; set; }
    }
}
