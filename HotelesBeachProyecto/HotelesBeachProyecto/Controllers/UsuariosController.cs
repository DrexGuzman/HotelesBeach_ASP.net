using HotelesBeachProyecto.Data;
using HotelesBeachProyecto.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace HotelesBeachProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private HotelAPI hotelAPI;
        private HttpClient client;

        public UsuariosController()
        {
            hotelAPI = new HotelAPI();
            client = hotelAPI.Inicial();
        }

        // GET: Usuarios/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            AutorizacionResponse autorizacion = null;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor, complete ambos campos.";
                return View();
            }
            HttpResponseMessage response = await client.PostAsync(
                $"/Usuarios/Login?email={email}&password={password}", null);
            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;
                autorizacion = JsonConvert.DeserializeObject<AutorizacionResponse>(resultado);

            }
            if (autorizacion != null && autorizacion.Resultado == true)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, email));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("CookieAuthentication", principal);
                HttpContext.Session.SetString("token", autorizacion.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Usuario o contraseña incorrectos.";
                return View();
            }

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string cedula, string telefono, string direccion, string email, string password, string confirmarPassword)
        {
            var user = new UsuarioDto();

            // Verificar si las contraseñas coinciden antes de enviar la solicitud
            if (password != confirmarPassword)
            {
                TempData["Mensaje"] = "Las contraseñas no coinciden";
                return View();
            }

            // Crear el objeto con los datos del usuario
            user.Email = email;
            user.Password = password;
            user.Cedula = cedula;
            user.Telefono = telefono;
            user.Direccion = direccion;

            try
            {
                // Enviar la solicitud POST
                var response = client.PostAsJsonAsync($"Usuarios/Agregar?confirmar={confirmarPassword}", user);

                await response;

                var resultado = response.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Usuario registrado con éxito";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Mensaje"] = "No se logro registrar el usuario";
                    return View();
                }

            }
            catch (Exception ex)
            {
                // Manejar excepciones de red u otros errores
                TempData["Mensaje"] = $"Error: {ex.Message}";
                return View();
            }
        }


        // GET: Usuarios/Logout
        public async Task<IActionResult> Logout()
        {
            //se cierra la sesion
            await HttpContext.SignOutAsync();
            //se ubica al usuario en la pagina de inicio
            return RedirectToAction("Index", "Home");
        }

    }
}
