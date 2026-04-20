namespace ApiSsistemaGestionInventarioRadiosTetra.Models
{
    public class DetalleContratoRequest
    {
        public int dec_id { get; set; }
        public int con_id { get; set; }
        public int mod_id { get; set; }
        public int dec_cantidad { get; set; }
        public decimal dec_precioUnitario { get; set; }
        public int usa_id { get; set; }
    }
}
