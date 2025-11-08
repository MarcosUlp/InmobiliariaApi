using System.ComponentModel.DataAnnotations;

namespace InmobiliariaApi.Models
{
    public class Propietario
    {
        public int PropietarioId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Clave { get; set; }

        public string? Telefono { get; set; }

        public ICollection<Inmueble>? Inmuebles { get; set; }
    }
}
