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
    public class PagosController : ControllerBase
    {
        private readonly InmobiliariaContext _context;

        public PagosController(InmobiliariaContext context)
        {
            _context = context;
        }

        // 🟢 1. Registrar un nuevo pago de un contrato
        [HttpPost]
        public async Task<IActionResult> CrearPago([FromBody] Pago pago)
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Verificamos que el contrato exista y pertenezca al propietario
            var contrato = await _context.Contratos
                .Include(c => c.Inmueble)
                .FirstOrDefaultAsync(c => c.ContratoId == pago.ContratoId &&
                                          c.Inmueble.PropietarioId == propietarioId);

            if (contrato == null)
                return BadRequest(new { mensaje = "El contrato no existe o no pertenece al propietario autenticado." });

            pago.FechaPago = pago.FechaPago.Date;

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Pago registrado correctamente.",
                pago = new
                {
                    pago.PagoId,
                    pago.ContratoId,
                    pago.FechaPago,
                    pago.Importe
                }
            });

        }

        // 🔵 2. Listar pagos de un contrato específico
        [HttpGet("contrato/{contratoId}")]
        public async Task<IActionResult> GetPagosPorContrato(int contratoId)
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Validamos que el contrato pertenezca al propietario
            var contrato = await _context.Contratos
                .Include(c => c.Inmueble)
                .FirstOrDefaultAsync(c => c.ContratoId == contratoId &&
                                          c.Inmueble.PropietarioId == propietarioId);

            if (contrato == null)
                return NotFound(new { mensaje = "Contrato no encontrado o no pertenece al propietario autenticado." });

            var pagos = await _context.Pagos
                .Where(p => p.ContratoId == contratoId)
                .Select(p => new
                {
                    p.PagoId,
                    p.FechaPago,
                    p.Importe
                }).ToListAsync();

            return Ok(pagos);
        }
    }
}
