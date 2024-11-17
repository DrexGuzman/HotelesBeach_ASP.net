using System.ComponentModel.DataAnnotations;

namespace ApiHotelesBeach.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debe ingresar una contraseña segura.")]
        public string Password { get; set; }

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
    }
}
