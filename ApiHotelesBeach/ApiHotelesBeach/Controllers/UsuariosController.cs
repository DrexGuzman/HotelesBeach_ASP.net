using ApiHotelesBeach.Data;
using ApiHotelesBeach.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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

        private string ValidarPassword(string password, string nombreCompleto)
        {
            // Verificar que la contraseña tenga 8 caracteres o más
            if (password.Length < 8)
            {
                return "La contraseña no puede ser menor a 8 caracteres";
            }

            // Verificar que la contraseña tenga al menos una letra en mayúscula
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return "La contraseña debe tener por lo menos una letra en mayúscula";
            }

            // Verificar que la contraseña tenga al menos una letra en minúscula
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return "La contraseña debe tener por lo menos una letra en minúscula";
            }

            // Verificar que la contraseña tenga al menos un número
            if (!Regex.IsMatch(password, @"\d"))
            {
                return "La contraseña debe tener por lo menos un número";
            }

            // Verificar que la contraseña no contenga el nombre del usuario
            if (password.ToLower().Contains(nombreCompleto.ToLower()))
            {
                return "La contraseña no debe de contener su nombre.";
            }

            // Verificar que la contraseña tenga por lo menos dos caracteres especiales
            if (Regex.Matches(password, @"[!@#$%^&*(),.?\':{ }|<>]").Count < 2)
            {
                return "La contraseña debe tener al menos dos caracteres especiales.";
            }

            return null;
        }

        [HttpPost("Agregar")]
        public async Task<string> Agregar(Usuario usuario, string confirmar)
        {
            string mensaje = "Debe ingresar la información completa del usuario";

            if (usuario == null)
            {
                return mensaje;
            }

            if (!usuario.Password.Equals(confirmar))
            {
                return "La confirmación de la contraseña ha fallado.";
            }

            var existentUser = _context.Usuarios.FirstOrDefault(x => x.Cedula == usuario.Cedula);
            if (existentUser != null)
            {
                return "Ya existe un usuario asociado a la cédula ingresada.";
            }

            // Consultar API de GOMETA para completar campos
            var (fullName, guessType) = await ConsultarApiGometa(usuario.Cedula);
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(guessType))
            {
                return "No se pudo encontrar información sobre el usuario en la API de GOMETA.";
            }

            usuario.NombreCompleto = fullName;
            usuario.TipoCedula = guessType;

            // Sobrescribir el valor de IsAdmin a false siempre
            usuario.IsAdmin = false;

            mensaje = ValidarPassword(usuario.Password, usuario.NombreCompleto);
            if (!string.IsNullOrEmpty(mensaje))
            {
                return mensaje;
            }

            try
            {
                usuario.FechaRegistro = DateTime.Now;
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                mensaje = $"Usuario {usuario.NombreCompleto} agregado exitosamente";
            }
            catch (Exception ex)
            {
                mensaje = $"Error al agregar el usuario {usuario.NombreCompleto}. Detalle: {ex.Message}";
            }

            return mensaje;
        }


        // Método para consultar la API de GOMETA
        private async Task<(string FullName, string GuessType)> ConsultarApiGometa(string cedula)
        {
            string apiUrl = $"https://apis.gometa.org/cedulas/{cedula}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        // Deserialización como objeto dinámico
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                        // Verificar si hay resultados
                        if (data.resultcount > 0 && data.results != null && data.results.Count > 0)
                        {
                            var firstResult = data.results[0];
                            return (firstResult.fullname.ToString(), firstResult.guess_type.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar la API: {ex.Message}");
                }
            }
            return (null, null);
        }


    }
}
