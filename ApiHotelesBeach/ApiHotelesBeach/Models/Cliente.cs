using System.ComponentModel.DataAnnotations;

namespace ApiHotelesBeach.Models
{
    public class Cliente
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(10)]
        public string TipoCedula { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [StringLength(15)]
        [Phone]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
