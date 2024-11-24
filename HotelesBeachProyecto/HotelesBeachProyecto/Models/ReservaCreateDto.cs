using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelesBeachProyecto.Models
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

        [Required]
        public string NombreFormaPago { get; set; }

        [Required]
        public string ClienteCedula { get; set; }
    }
}
