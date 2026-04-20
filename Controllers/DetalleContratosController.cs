using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSsistemaGestionInventarioRadiosTetra.Controllers
{
    [ApiController]
    [Route("api/contratoDetalle")]
    public class DetalleContratosController : Controller
    {
        private readonly IDetalleContratoRepository _detalleContratoRepository;

        public DetalleContratosController(IDetalleContratoRepository detalleContratoRepository)
        {
            _detalleContratoRepository = detalleContratoRepository;
        }

        [HttpGet("contrato/{conId:int}")]
        public async Task<IActionResult> ListarPorContrato(int conId)
        {
            var data = await _detalleContratoRepository.ListarPorContratoAsync(conId);
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var item = await _detalleContratoRepository.ObtenerAsync(id);

            if (item == null)
                return NotFound(new { message = "Detalle de contrato no encontrado" });

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] DetalleContratoRequest request)
        {
            request.dec_id = 0;
            int nuevoId = await _detalleContratoRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Detalle de contrato registrado correctamente",
                dec_id = nuevoId
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] DetalleContratoRequest request)
        {
            request.dec_id = id;
            int detalleId = await _detalleContratoRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Detalle de contrato actualizado correctamente",
                dec_id = detalleId
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            bool ok = await _detalleContratoRepository.EliminarLogicoAsync(id);

            if (!ok)
                return NotFound(new { message = "Detalle no encontrado o ya fue eliminado" });

            return Ok(new { message = "Detalle eliminado lógicamente correctamente" });
        }
    }
}
