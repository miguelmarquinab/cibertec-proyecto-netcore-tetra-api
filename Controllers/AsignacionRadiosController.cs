using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSsistemaGestionInventarioRadiosTetra.Controllers
{
    [ApiController]
    [Route("api/asignacionradio")]
    public class AsignacionRadiosController : Controller
    {
        private readonly IAsignacionRadioRepository _asignacionRadioRepository;

        public AsignacionRadiosController(IAsignacionRadioRepository asignacionRadioRepository)
        {
            _asignacionRadioRepository = asignacionRadioRepository;
        }

        [HttpGet("detalle/{decId:int}")]
        public async Task<IActionResult> ListarPorDetalle(int decId)
        {
            var data = await _asignacionRadioRepository.ListarPorDetalleAsync(decId);
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var item = await _asignacionRadioRepository.ObtenerAsync(id);

            if (item == null)
                return NotFound(new { message = "Asignación no encontrada" });

            return Ok(item);
        }

        [HttpGet("disponibles/modelo/{modId:int}")]
        public async Task<IActionResult> ListarDisponiblesPorModelo(int modId)
        {
            var data = await _asignacionRadioRepository.ListarDisponiblesPorModeloAsync(modId);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] AsignacionRadioRequest request)
        {
            request.asr_id = 0;
            int nuevoId = await _asignacionRadioRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Radio asignado correctamente",
                asr_id = nuevoId
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] AsignacionRadioRequest request)
        {
            request.asr_id = id;
            int asignacionId = await _asignacionRadioRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Asignación actualizada correctamente",
                asr_id = asignacionId
            });
        }

        [HttpPut("devolver/{id:int}")]
        public async Task<IActionResult> Devolver(int id, [FromBody] AsignacionRadioDevolucionRequest request)
        {
            bool ok = await _asignacionRadioRepository.DevolverAsync(id, request);

            if (!ok)
                return NotFound(new { message = "Asignación no encontrada o no se pudo devolver" });

            return Ok(new { message = "Radio devuelto correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            bool ok = await _asignacionRadioRepository.EliminarLogicoAsync(id);

            if (!ok)
                return NotFound(new { message = "Asignación no encontrada o ya fue eliminada" });

            return Ok(new { message = "Asignación eliminada lógicamente correctamente" });
        }
    }
}
