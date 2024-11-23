using HotelesBeachProyecto.Data;
using HotelesBeachProyecto.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HotelesBeachProyecto.Controllers
{
    public class GestionarReservasController : Controller
    {
        private HotelAPI hotelAPI;
        private HttpClient client;

        public GestionarReservasController()
        {
            hotelAPI = new HotelAPI();
            client = hotelAPI.Inicial();
        }
        public async Task<IActionResult> Index()
        {
            List<Reserva> reservas = new List<Reserva>();
            client.DefaultRequestHeaders.Authorization = AutorizacionToken();
            HttpResponseMessage response = await client.GetAsync("/Reservas/Listado");

            if (response.StatusCode.ToString().Equals("Unauthorized"))
            {

                return RedirectToAction("Login", "Usuarios");
            }

            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;

                reservas = JsonConvert.DeserializeObject<List<Reserva>>(resultado);

            }

            return View(reservas);
        }



        private AuthenticationHeaderValue AutorizacionToken()
        {
            //se extrae el token almacenado dentro de la sesión
            var token = HttpContext.Session.GetString("token");

            //Variable para almacenar el token de autenticación
            AuthenticationHeaderValue authentication = null;

            if (token != null && token.Length != 0)
            {
                //Se almacena el token  otorgado por la API
                authentication = new AuthenticationHeaderValue("Bearer", token);
            }

            //se retorna la información
            return authentication;
        }
    }
}
