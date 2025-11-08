using InmobiliariaApi.Data;
using InmobiliariaApi.Models;
using InmobiliariaApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InmobiliariaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly InmobiliariaContext _context;
        private readonly IConfiguration _config;

        public AuthController(InmobiliariaContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] PropietarioRegistroDTO dto)
        {
            if (await _context.Propietarios.AnyAsync(p => p.Email == dto.Email))
            {
                return BadRequest(new { mensaje = "El email ya está registrado." });
            }

            var propietario = new Propietario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Clave = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
                Telefono = dto.Telefono
            };

            _context.Propietarios.Add(propietario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Propietario registrado correctamente." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PropietarioLoginDTO dto)
        {
            var propietario = await _context.Propietarios.FirstOrDefaultAsync(p => p.Email == dto.Email);
            if (propietario == null || !BCrypt.Net.BCrypt.Verify(dto.Clave, propietario.Clave))
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });
            }

            // Crear token JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, propietario.PropietarioId.ToString()),
                new Claim(ClaimTypes.Email, propietario.Email),
                new Claim(ClaimTypes.Name, propietario.Nombre)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                propietario = new { propietario.PropietarioId, propietario.Nombre, propietario.Email }
            });
        }
    }
}
