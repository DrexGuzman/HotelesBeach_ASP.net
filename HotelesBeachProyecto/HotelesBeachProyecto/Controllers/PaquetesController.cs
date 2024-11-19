using Microsoft.AspNetCore.Mvc;
using HotelesBeachProyecto.Models;
using HotelesBeachProyecto.Data;

namespace HotelesBeachProyecto.Controllers
{
    public class PaquetesController : Controller
    {
        private readonly DbContextHotel _context;

        // GET: Paquetes
        public ActionResult Index()
        {
            var paquetes = new List<Paquete>
    {
        new Paquete { Id = 1, Nombre = "Todo Incluido", Costo = 450, Prima = 45, Mensualidades = 24 },
        new Paquete { Id = 2, Nombre = "Alimentación", Costo = 275, Prima = 35, Mensualidades = 18 },
        new Paquete { Id = 3, Nombre = "Hospedaje", Costo = 210, Prima = 15, Mensualidades = 12 }
    };
            //var paquetes = _context.Paquetes.ToList();
            return View(paquetes);
        }

        // GET: Paquetes/Reservar/{id}
        public ActionResult Reservar(int id)
        {
            var paquete = _context.Paquetes.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // POST: Paquetes/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservar(int id, string nombreCliente)
        {
            var paquete = _context.Paquetes.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            // Aquí se puede implementar la lógica para almacenar la reserva
            ViewBag.Message = $"¡Reserva realizada con éxito para el paquete {paquete.Nombre} a nombre de {nombreCliente}!";
            return RedirectToAction("Index");
        }

    }
}
