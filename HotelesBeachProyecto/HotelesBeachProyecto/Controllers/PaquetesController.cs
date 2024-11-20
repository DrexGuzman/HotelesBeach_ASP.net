using Microsoft.AspNetCore.Mvc;
using HotelesBeachProyecto.Models;
using HotelesBeachProyecto.Data;
using Newtonsoft.Json;

namespace HotelesBeachProyecto.Controllers
{
    public class PaquetesController : Controller
    {
        private HotelAPI hotelAPI;
        private HttpClient client;

        public PaquetesController()
        {
                hotelAPI = new HotelAPI();
                client = hotelAPI.Inicial();
        }

        // GET: Paquetes
        public async Task<IActionResult> Index()
        {

            List<Paquete> paquetes = new List<Paquete>();

            //se utiliza el metodo de la api
            HttpResponseMessage response = await client.GetAsync("/Paquetes/Listado");

            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;

                paquetes = JsonConvert.DeserializeObject<List<Paquete>>(resultado);
            }

            return View(paquetes);

        }

        // GET: Paquetes/Reservar/{id}
        public ActionResult Reservar(int id)
        {
            //var paquete = _context.Paquetes.Find(id);
            //if (paquete == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        // POST: Paquetes/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservar(int id, string nombreCliente)
        {
            //var paquete = _context.Paquetes.Find(id);
            //if (paquete == null)
            //{
            //    return NotFound();
            //}

            //// Aquí se puede implementar la lógica para almacenar la reserva
            //ViewBag.Message = $"¡Reserva realizada con éxito para el paquete {paquete.Nombre} a nombre de {nombreCliente}!";
            return RedirectToAction("Index");
        }

    }
}
