using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Models
{
    public class Inmueble
    {
        public int InmuebleId { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public int Ambientes { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public string? Imagen { get; set; }

        public bool Habilitado { get; set; } = false;

        [ForeignKey("Propietario")]
        public int PropietarioId { get; set; }

        public Propietario? Propietario { get; set; }

        public ICollection<Contrato>? Contratos { get; set; }
    }
}
