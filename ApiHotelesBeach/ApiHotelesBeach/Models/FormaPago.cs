using System.ComponentModel.DataAnnotations;

namespace ApiHotelesBeach.Models
{
    public class FormaPago
    {
        [Key]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe indicar el nombre de la forma de pago")]
        [StringLength(100)]
        public string Nombre { get; set; }

        //[Required(ErrorMessage = "Debe indicar el número de cheque o tarjeta")]
        public int Numero { get; set; }

        [StringLength(100)]
        public string? Banco { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "El CVV debe tener exactamente 3 dígitos.")]
        public string? CVV { get; set; }

        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "La fecha debe estar en formato MM/YY.")]
        public string? FechaExpiracion { get; set; }

        [StringLength(100)]
        public string? NombreTitular { get; set; }
    }
}
