using ApiHotelesBeach.Data;
using ApiHotelesBeach.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiHotelesBeach.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        private readonly DbContextHotel _context = null;

        public UsuariosController(DbContextHotel pContext)
        {
            _context = pContext;
        }
     
        [HttpGet("Listado")]
        public List<Usuario> Listado()
        {
            List<Usuario> lista = null;
            lista = _context.Usuarios.ToList();
            return lista;
        }

        [HttpPost("Agregar")]
        public async Task<string> Agregar(Usuario temp)
        {
            string mensaje = $"Usuario {temp.NombreCompleto} agregado exitosamente";
            if (temp == null)
            {
                return mensaje;
            }
            else
            {
                var userExist = _context.Usuarios.Any(x => x.Email == temp.Email);
                if (userExist)
                {
                    mensaje = "Correo electronico en uso";
                    return mensaje;
                }
                try
                {
                    if (!ModelState.IsValid)
                    {
                        mensaje = "Verifique los datos ingresados";
                        if (temp.Email == "")
                        {
                            mensaje = "Debe de ingresar un Email";
                        }
                        if (temp.NombreCompleto == "")
                        {
                            mensaje = "Debe de ingresar el NombreCompleto";
                        }
                        return mensaje;
                    }

                    _context.Usuarios.Add(temp);
                    await _context.SaveChangesAsync();

                    return mensaje;


                }
                catch (Exception ex)
                {
                    mensaje = $"Error al agregar el usuario {temp.NombreCompleto} detalle {ex.InnerException}";
                }
                return mensaje;
            }
        }
    }
}
