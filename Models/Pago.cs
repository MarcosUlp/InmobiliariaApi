using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaApi.Models
{
    public class Pago
    {
        public int PagoId { get; set; }

        [ForeignKey("Contrato")]
        public int ContratoId { get; set; }

        public Contrato? Contrato { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal Importe { get; set; }
    }
}
