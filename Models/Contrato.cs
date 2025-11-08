using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Models
{
    public class Contrato
    {
        public int ContratoId { get; set; }

        [ForeignKey("Inmueble")]
        public int InmuebleId { get; set; }

        public Inmueble? Inmueble { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public ICollection<Pago>? Pagos { get; set; }
    }
}
