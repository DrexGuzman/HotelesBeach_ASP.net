using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiHotelesBeach.Models
{
    public class ReservaCreateDto
    {
        [Required]
        [Range(1, 365)]
        public int CantidadNoches { get; set; }

        [Required]
        [Range(1, 8)]
        public int CantidadPersonas { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Descuento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal MontoRebajado { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal MontoFinal { get; set; }

        [Required]
        public int PaqueteId { get; set; }

        [Required]
        public int FormaPagoId { get; set; }

        [Required]
        public string ClienteCedula { get; set; }
    }
}
