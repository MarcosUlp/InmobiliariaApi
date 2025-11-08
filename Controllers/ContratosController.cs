using InmobiliariaApi.Data;
using InmobiliariaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InmobiliariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContratosController : ControllerBase
    {
        private readonly InmobiliariaContext _context;

        public ContratosController(InmobiliariaContext context)
        {
            _context = context;
        }

        // 🟢 1. Crear nuevo contrato para un inmueble del propietario autenticado
        [HttpPost]
        public async Task<IActionResult> CrearContrato([FromBody] Contrato contrato)
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Validar inmueble
            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.InmuebleId == contrato.InmuebleId && i.PropietarioId == propietarioId);

            if (inmueble == null)
                return BadRequest(new { mensaje = "El inmueble no existe o no pertenece al propietario autenticado." });

            // Crear contrato
            contrato.FechaInicio = contrato.FechaInicio.Date;
            contrato.FechaFin = contrato.FechaFin.Date;

            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Contrato creado correctamente.",
                contrato = new
                {
                    contrato.ContratoId,
                    contrato.InmuebleId,
                    contrato.FechaInicio,
                    contrato.FechaFin,
                    contrato.Monto
                }
            });
        }

        // 🔵 2. Listar contratos por inmueble (solo los del propietario autenticado)
        [HttpGet("inmueble/{id}")]
        public async Task<IActionResult> GetContratosPorInmueble(int id)
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.InmuebleId == id && i.PropietarioId == propietarioId);

            if (inmueble == null)
                return NotFound(new { mensaje = "Inmueble no encontrado o no pertenece al propietario autenticado." });

            var contratos = await _context.Contratos
                .Include(c => c.Pagos)
                .Where(c => c.InmuebleId == id)
                .Select(c => new
                {
                    c.ContratoId,
                    c.FechaInicio,
                    c.FechaFin,
                    c.Monto,
                    Pagos = c.Pagos.Select(p => new
                    {
                        p.PagoId,
                        p.FechaPago,
                        p.Importe
                    }).ToList()
                })
                .ToListAsync();

            return Ok(contratos);
        }
    }
}
