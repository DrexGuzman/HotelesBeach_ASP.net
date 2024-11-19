using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiHotelesBeach.Dto
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
        public int PaqueteId { get; set; }

        [StringLength(100)]
        public string? NombreFormaPago { get; set; }

        //[Required(ErrorMessage = "Debe indicar el número de cheque o tarjeta")]
        public int? Numero { get; set; }

        [StringLength(100)]
        public string? Banco { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "El CVV debe tener exactamente 3 dígitos.")]
        public string? CVV { get; set; }

        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "La fecha debe estar en formato MM/YY.")]
        public string? FechaExpiracion { get; set; }

        [StringLength(100)]
        public string? NombreTitular { get; set; }

        [Required]
        public string ClienteCedula { get; set; }
    }
}
