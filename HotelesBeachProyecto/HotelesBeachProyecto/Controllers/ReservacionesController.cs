using Microsoft.AspNetCore.Mvc;
using HotelesBeachProyecto.Models;

namespace HotelesBeachProyecto.Controllers
{
    public class ReservacionesController : Controller
    {
        public IActionResult NuevaReserva()
        {
            var viewModel = new Reservacion
            {
                Descuento = 0,
                TotalPagar = 450, // Valor predeterminado
                Prima = 202.5m,  // Valor predeterminado
                CuotasPendientes = 24,
                MontoCuota = 10.31m
            };

            return View(viewModel);
        }

        // Método para procesar el formulario
        [HttpPost]
        public IActionResult ProcesarReserva(Reservacion model)
        {
            if (ModelState.IsValid)
            {
                // Procesar la información (guardar en base de datos, etc.)
                // Redirigir o mostrar confirmación
                return RedirectToAction("Confirmacion");
            }

            // Si hay errores, regresar a la misma vista con los datos ingresados
            return View("NuevaReserva", model);
        }

        // Método de confirmación (opcional)
        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}
