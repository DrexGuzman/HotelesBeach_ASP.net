using System.ComponentModel.DataAnnotations;

namespace HotelesBeachProyecto.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(10)]
        public string? TipoCedula { get; set; }

        [Required]
        [StringLength(100)]
        public string? NombreCompleto { get; set; }

        [StringLength(15)]
        [Phone]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debe ingresar una contraseña segura")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Indique la fecha y hora de registro del usuario")]
        public DateTime FechaRegistro { get; set; }

        public bool Confirmar(string pw)
        {
            bool confirmado = false;
            if (Password != null)
            {
                if (Password.Equals(pw))
                {
                    confirmado = true;

                }
            }
            return confirmado;
        }

        [Required(ErrorMessage = "Debe indicar si el usuario es o no un administrador")]
        public bool IsAdmin { get; set; }
    }
}
