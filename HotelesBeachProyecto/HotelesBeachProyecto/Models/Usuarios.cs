using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace HotelesBeachProyecto.Models
{
    public class Usuarios
    {

        [Key]
        public string? Cedula { get; set; }

        public string? TipoCedula { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string? Email { get; set; }


    }
}
