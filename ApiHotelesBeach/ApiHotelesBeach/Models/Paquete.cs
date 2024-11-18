using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiHotelesBeach.Models
{
    public class Paquete
    {

        [Key]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe indicar el nombre del paquete")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        [Required(ErrorMessage = "Debe indicar el costo del paquete")]
        public decimal Costo { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        [Required(ErrorMessage = "Debe indicar la prima del paquete")]
        public decimal Prima { get; set; }

        [Required(ErrorMessage = "Debe indicar las mensualidades del paquete")]
        public int Mensualidades { get; set; }
    }
}
