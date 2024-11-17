using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiHotelesBeach.Models
{
    public class Reservacion
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? PaqueteSeleccionado { get; set; }

        [Required]
        [Range(1, 365)]
        public int CantidadNoches { get; set; }

        [Required]
        [Range(1, 20)]
        public int CantidadPersonas { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPagar { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Prima { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CuotasPendientes { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MontoCuota { get; set; }

        //Clave foranea, se configura en el dbcontext
        [Required]
        [ForeignKey("Usuario")]
        public string UsuarioCedula { get; set; }

        //Esto permite acceder al usuario que esta relacionado en la reservacion
        public Usuario Usuario { get; set; }
    }
}
