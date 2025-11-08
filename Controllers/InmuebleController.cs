using InmobiliariaApi.Data;
using InmobiliariaApi.Dtos;
using InmobiliariaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InmobiliariaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InmueblesController : ControllerBase
    {
        private readonly InmobiliariaContext _context;
        private readonly IWebHostEnvironment _env;

        public InmueblesController(InmobiliariaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 🟢 1. Listar inmuebles del propietario autenticado
        [HttpGet]
        public IActionResult GetInmuebles()
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var inmuebles = _context.Inmuebles
                .Where(i => i.PropietarioId == propietarioId)
                .Select(i => new InmuebleDto
                {
                    Id = i.InmuebleId,
                    Direccion = i.Direccion,
                    Ambientes = i.Ambientes,
                    Precio = i.Precio,
                    Habilitado = i.Habilitado,
                    Imagen = i.Imagen
                }).ToList();

            return Ok(inmuebles);
        }

        // 🟡 2. Agregar nuevo inmueble (por defecto deshabilitado)
        [HttpPost]
        public async Task<IActionResult> CrearInmueble([FromForm] CrearInmuebleDto dto)
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            string? rutaImagen = null;

            if (dto.Imagen != null)
            {
                string carpeta = Path.Combine(_env.WebRootPath, "imagenes_inmuebles");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombreArchivo = Guid.NewGuid() + Path.GetExtension(dto.Imagen.FileName);
                string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await dto.Imagen.CopyToAsync(stream);
                }

                rutaImagen = "/imagenes_inmuebles/" + nombreArchivo;
            }

            var inmueble = new Inmueble
            {
                Direccion = dto.Direccion,
                Ambientes = dto.Ambientes,
                Precio = dto.Precio,
                Imagen = rutaImagen,
                PropietarioId = propietarioId,
                Habilitado = false // Por defecto
            };

            _context.Inmuebles.Add(inmueble);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Inmueble agregado correctamente y deshabilitado por defecto." });
        }

        // 🔵 3. Habilitar/Deshabilitar inmueble
        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            int propietarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var inmueble = await _context.Inmuebles.FindAsync(id);
            if (inmueble == null || inmueble.PropietarioId != propietarioId)
                return NotFound(new { mensaje = "Inmueble no encontrado o no pertenece al propietario." });

            inmueble.Habilitado = !inmueble.Habilitado;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Inmueble {(inmueble.Habilitado ? "habilitado" : "deshabilitado")} correctamente." });
        }

    }
}
