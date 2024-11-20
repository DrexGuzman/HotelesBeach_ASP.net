using ApiHotelesBeach.Data;
using ApiHotelesBeach.Dto;
using ApiHotelesBeach.Models;
using ApiHotelesBeach.Models.Custom;
using ApiHotelesBeach.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ApiHotelesBeach.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        private readonly DbContextHotel _context = null;
        private readonly IAutorizacionServices _autorizacionServices;

        public UsuariosController(DbContextHotel pContext, IAutorizacionServices autorizacionServices)
        {
            _context = pContext;
            _autorizacionServices = autorizacionServices;
        }

        [Authorize]
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
        public async Task<IActionResult> Agregar([FromBody] UsuarioDto usuarioDto, string confirmar)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Debe ingresar la información completa del usuario");
            }

            if (usuarioDto.Cedula.Length < 9)
            {
                return BadRequest("La cédula del usuario debe tener 9 carácteres o más.");
            }

            if (!usuarioDto.Password.Equals(confirmar))
            {
                return BadRequest("La confirmación de la contraseña ha fallado.");
            }

            var existentUser = _context.Usuarios.FirstOrDefault(x => x.Cedula == usuarioDto.Cedula);
            if (existentUser != null)
            {
                return Conflict("Ya existe un usuario asociado a la cédula ingresada.");
            }

            // Consultar API de GOMETA para completar campos
            var (fullName, guessType) = await ConsultarApiGometa(usuarioDto.Cedula);
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(guessType))
            {
                return BadRequest("No se pudo encontrar información sobre el usuario en la API de GOMETA.");
            }

            // Crear la instancia de Usuario
            var usuario = new Usuario
            {
                Cedula = usuarioDto.Cedula,
                Telefono = usuarioDto.Telefono,
                Direccion = usuarioDto.Direccion,
                Email = usuarioDto.Email,
                Password = usuarioDto.Password,
                NombreCompleto = fullName,
                TipoCedula = guessType,
                IsAdmin = false, // Por defecto todos los usuarios son no administradores
                FechaRegistro = DateTime.Now
            };

            string mensaje = ValidarPassword(usuario.Password, usuario.NombreCompleto);
            if (!string.IsNullOrEmpty(mensaje))
            {
                return BadRequest(mensaje);
            }

            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return Ok($"Usuario {usuario.NombreCompleto} agregado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el usuario {usuario.NombreCompleto}. Detalle: {ex.Message}");
            }
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

        [Authorize]
        [HttpGet("Buscar/{cedula}")]
        public IActionResult Buscar(string cedula)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Cedula == cedula);

            if (usuario == null)
            {
                return NotFound($"Usuario con cédula {cedula} no encontrado.");
            }

            return Ok(usuario);
        }

        [Authorize]
        [HttpPut("Actualizar/{cedula}")]
        public async Task<IActionResult> Actualizar(string cedula, [FromBody] UsuarioEditarDto usuarioDto)
        {
            if (usuarioDto == null)
            {
                return BadRequest("Debe ingresar la información completa del usuario");
            }

            // Buscar al usuario por la cédula proporcionada
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Cedula == cedula);
            if (usuario == null)
            {
                return NotFound($"Usuario con cédula {cedula} no encontrado.");
            }

            // Verificar si el correo electrónico es único
            var existentUserByEmail = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioDto.Email && x.Cedula != cedula);
            if (existentUserByEmail != null)
            {
                return Conflict("Ya existe un usuario asociado al correo electrónico ingresado.");
            }

            // Actualizar los campos del usuario
            usuario.Telefono = usuarioDto.Telefono;
            usuario.Direccion = usuarioDto.Direccion;
            usuario.Email = usuarioDto.Email;
            usuario.IsAdmin = usuarioDto.IsAdmin;

            string mensaje = ValidarPassword(usuario.Password, usuario.NombreCompleto);
            if (!string.IsNullOrEmpty(mensaje))
            {
                return BadRequest(mensaje);
            }

            try
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return Ok($"Usuario {usuario.NombreCompleto} actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el usuario {usuario.NombreCompleto}. Detalle: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("Eliminar/{cedula}")]
        public async Task<IActionResult> Eliminar(string cedula)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Cedula == cedula);
            if (usuario == null)
            {
                return NotFound($"Usuario con cédula {cedula} no encontrado.");
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return Ok($"Usuario {usuario.NombreCompleto} eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el usuario {usuario.NombreCompleto}. Detalle: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var temp = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password.Equals(password));
            if (temp == null)
            {
                return Unauthorized(new AutorizacionResponse() { Token = "", Msj = "No autorizado", Resultado = false });
            }
            else
            {
                var autorizado = await _autorizacionServices.DevolverToken(temp);
                if (autorizado == null)
                {
                    return Unauthorized(new AutorizacionResponse() { Token = "", Msj = "No autorizado", Resultado = false });
                }
                else
                {
                    return Ok(autorizado);
                }
            }
        }
    }
}
