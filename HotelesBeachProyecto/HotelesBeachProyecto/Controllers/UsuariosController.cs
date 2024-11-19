using HotelesBeachProyecto.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelesBeachProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly DbContextHotel _context;

        // GET: Usuarios/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor, complete ambos campos.";
                return View();
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario == null || !usuario.Confirmar(password))
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            // Autenticar al usuario y redirigir
            //Session["UsuarioId"] = usuario.Cedula;
            //Session["NombreUsuario"] = usuario.NombreCompleto;

            return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Logout
        public ActionResult Logout()
        {
            //Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
