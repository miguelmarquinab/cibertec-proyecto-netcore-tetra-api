namespace ApiSsistemaGestionInventarioRadiosTetra.Models.Dto
{
    public class UsuarioLoginDto
    {
        public int usa_id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string usa_nombres { get; set; } = string.Empty;
        public string usa_apellidopaterno { get; set; } = string.Empty;
        public string usa_apellidomaterno { get; set; } = string.Empty;
        public string? usa_estado { get; set; }
        public string? usa_activo { get; set; }
        public int? rol { get; set; }
        public string? rol_nombre { get; set; }
    }
}
