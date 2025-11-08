using InmobiliariaApi.Data;
using InmobiliariaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BCrypt.Net;
using InmobiliariaApi.Dtos;

namespace InmobiliariaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔒 Requiere token para todos los endpoints
    public class PropietarioController : ControllerBase
    {
        private readonly InmobiliariaContext _context;

        public PropietarioController(InmobiliariaContext context)
        {
            _context = context;
        }

        // 📍 Obtener perfil del propietario autenticado
        [HttpGet("perfil")]
        public async Task<IActionResult> ObtenerPerfil()
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var propietario = await _context.Propietarios
                .Where(p => p.PropietarioId == propietarioId)
                .Select(p => new
                {
                    p.PropietarioId,
                    p.Nombre,
                    p.Email,
                    p.Telefono
                })
                .FirstOrDefaultAsync();

            if (propietario == null)
                return NotFound(new { mensaje = "Propietario no encontrado." });

            return Ok(propietario);
        }

        // editar datos del propietario


    [HttpPut("editar")]
    public async Task<IActionResult> EditarPerfil([FromBody] EditarPerfilDto datos)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var propietario = await _context.Propietarios.FindAsync(propietarioId);
        if (propietario == null)
            return NotFound(new { mensaje = "Propietario no encontrado." });

        if (!string.IsNullOrEmpty(datos.Nombre))
            propietario.Nombre = datos.Nombre;

        if (!string.IsNullOrEmpty(datos.Email))
            propietario.Email = datos.Email;

        if (!string.IsNullOrEmpty(datos.Telefono))
            propietario.Telefono = datos.Telefono;

        _context.Propietarios.Update(propietario);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Perfil actualizado correctamente." });
    }

    // 🔑 Cambiar clave
    [HttpPut("cambiar-clave")]
    public async Task<IActionResult> CambiarClave([FromBody] CambiarClaveDTO dto)
    {
        int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var propietario = await _context.Propietarios.FindAsync(propietarioId);
        if (propietario == null)
            return NotFound(new { mensaje = "Propietario no encontrado." });

        if (!BCrypt.Net.BCrypt.Verify(dto.ClaveActual, propietario.Clave))
            return BadRequest(new { mensaje = "La clave actual es incorrecta." });

        propietario.Clave = BCrypt.Net.BCrypt.HashPassword(dto.NuevaClave);

        _context.Propietarios.Update(propietario);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Clave actualizada correctamente." });
    }
}

// DTO para cambiar clave
public class CambiarClaveDTO
{
    public string ClaveActual { get; set; }
    public string NuevaClave { get; set; }
}
}
