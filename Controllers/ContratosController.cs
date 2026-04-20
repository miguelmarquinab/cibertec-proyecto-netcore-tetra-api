using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSsistemaGestionInventarioRadiosTetra.Controllers
{

    [ApiController]
    [Route("api/contrato")]
    public class ContratosController : ControllerBase
    {
        private readonly IContratoRepository _contratoRepository;

        public ContratosController(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] int clienteId = 0,
            [FromQuery] string? estado = "",
            [FromQuery] string? fechaInicio = "",
            [FromQuery] string? fechaFin = "")
        {
            var data = await _contratoRepository.ListarAsync(clienteId, estado, fechaInicio, fechaFin);
            return Ok(data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var item = await _contratoRepository.ObtenerAsync(id);

            if (item == null)
                return NotFound(new { message = "Contrato no encontrado" });

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] ContratoRequest request)
        {
            request.con_id = 0;
            int nuevoId = await _contratoRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Contrato registrado correctamente",
                con_id = nuevoId
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ContratoRequest request)
        {
            request.con_id = id;
            int contratoId = await _contratoRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Contrato actualizado correctamente",
                con_id = contratoId
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarLogico(int id)
        {
            bool ok = await _contratoRepository.EliminarLogicoAsync(id);

            if (!ok)
                return NotFound(new { message = "Contrato no encontrado o ya fue eliminado" });

            return Ok(new { message = "Contrato eliminado lógicamente correctamente" });
        }
    }
}
