using ApiSsistemaGestionInventarioRadiosTetra.Models;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories;
using ApiSsistemaGestionInventarioRadiosTetra.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiSsistemaGestionInventarioRadiosTetra.Controllers
{
    [ApiController]
    [Route("api/radio")]
    public class RadiosController : Controller
    {
        private readonly IRadioRepository _radioRepository;

        public RadiosController(IRadioRepository radiodRepository)
        {
            _radioRepository = radiodRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? texto)
        {
            var data = await _radioRepository.ListarAsync(texto);
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var item = await _radioRepository.ObtenerAsync(id);

            if (item == null)
                return NotFound(new { message = "Radio no encontrada" });

            return Ok(item);
        }

        [HttpGet("modelos")]
        public async Task<IActionResult> Modelos()
        {
            var data = await _radioRepository.ListarModelosAsync();
            return Ok(data);
        }

        [HttpGet("estados")]
        public async Task<IActionResult> Estados()
        {
            var data = await _radioRepository.ListarEstadosAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] RadioRequest request)
        {
            request.rad_id = 0;
            int nuevoId = await _radioRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Radio registrada correctamente",
                rad_id = nuevoId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] RadioRequest request)
        {
            request.rad_id = id;
            int radioId = await _radioRepository.GuardarAsync(request);

            return Ok(new
            {
                message = "Radio actualizada correctamente",
                rad_id = radioId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool ok = await _radioRepository.EliminarAsync(id);

            if (!ok)
                return NotFound(new { message = "Radio no encontrada o no se pudo eliminar" });

            return Ok(new { message = "Radio eliminada correctamente" });
        }
    }
}
