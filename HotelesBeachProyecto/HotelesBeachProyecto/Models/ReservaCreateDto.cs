using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

<<<<<<<< HEAD:ApiHotelesBeach/ApiHotelesBeach/Dto/ReservaCreateDto.cs
namespace ApiHotelesBeach.Dto
========
namespace HotelesBeachProyecto.Models
>>>>>>>> dev:HotelesBeachProyecto/HotelesBeachProyecto/Models/ReservaCreateDto.cs
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
        public int FormaPagoId { get; set; }

        [Required]
        public string ClienteCedula { get; set; }
    }
}
