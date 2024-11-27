using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelesBeachProyecto.Data; // Reemplaza con tu contexto de base de datos
using HotelesBeachProyecto.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace HotelesBeachProyecto.Controllers
{
    public class ReservasController : Controller
    {
        private HotelAPI hotelAPI;
        private HttpClient client;

        // Constructor: Recibe el contexto de la base de datos
        public ReservasController()
        {
            hotelAPI = new HotelAPI();
            client = hotelAPI.Inicial();
        }

        // GET: Reservas
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


        // GET: Reservas/Create
        public async Task<IActionResult> Create()
        {
            List<Paquete> paquetes = new List<Paquete>();

            // Llamar al método de la API
            HttpResponseMessage response = await client.GetAsync("/Paquetes/Listado");

            if (response.StatusCode.ToString().Equals("Unauthorized"))
            {

                return RedirectToAction("Login", "Usuarios");
            }

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                paquetes = JsonConvert.DeserializeObject<List<Paquete>>(resultado);
            }

            var viewModel = new ReservaViewModel
            {
                reserva = new ReservaCreateDto(), // Inicializa un objeto Reserva vacío
                paquete = paquetes
            };

            return View(viewModel);
        }

        // POST: Reservas/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,CantidadNoches,CantidadPersonas,Descuento,MontoRebajado,MontoFinal,PaqueteId,FormaPagoId,ClienteCedula")] Reserva reserva)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         // Calcula los montos automáticamente
        //         reserva.MontoRebajado = CalculaMontoRebajado(reserva.CantidadNoches, reserva.PaqueteId, reserva.Descuento);
        //         reserva.MontoFinal = CalculaMontoFinal(reserva.MontoRebajado, reserva.Descuento);

        //         _context.Add(reserva);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }

        //     ViewData["PaqueteId"] = new SelectList(_context.Paquetes, "Id", "Nombre", reserva.PaqueteId);
        //     ViewData["FormaPagoId"] = new SelectList(_context.FormasPago, "Id", "Nombre", reserva.FormaPagoId);
        //     return View(reserva);
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] ReservaViewModel viewModel)
        {
            ReservaCreateDto reserva = new ReservaCreateDto();
            reserva = viewModel.reserva;


            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var agregar = client.PostAsJsonAsync<ReservaCreateDto>("/Reservas/Agregar", reserva);
            await agregar;

            var resultado = agregar.Result;

            if (resultado.StatusCode.ToString().Equals("Unauthorized"))
            {

                return RedirectToAction("Login", "Usuarios");
            }

            if (resultado.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Se realizo la reserva con éxito";
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["Mensaje"] = "No se encontro la cedula";
                 return RedirectToAction("Create");
            }
        }

        private decimal CalculaMontoRebajado(int cantidadNoches, int paqueteId, decimal descuento)
        {
            // Implementa la lógica para calcular el monto rebajado
            //decimal precioBase = _context.Paquetes.Find(paqueteId)?.Costo ?? 0;
            //return cantidadNoches * precioBase * (1 - (descuento / 100));
            return 1;
        }

        private decimal CalculaMontoFinal(decimal montoRebajado, decimal descuento)
        {
            // Implementa lógica adicional si es necesario
            return montoRebajado;
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