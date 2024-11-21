using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelesBeachProyecto.Data;
using HotelesBeachProyecto.Models;
using Newtonsoft.Json;

namespace HotelesBeachProyecto.Controllers
{
    public class GestionarPaquetesController : Controller
    {
        private readonly DbContextHotel _context;
        private HotelAPI hotelAPI;
        private HttpClient client;


        public GestionarPaquetesController(DbContextHotel context)
        {
            _context = context;
            hotelAPI = new HotelAPI();
            client = hotelAPI.Inicial();
        }

        // GET: GestionarPaquetes
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

        // GET: GestionarPaquetes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] Paquete pPaquetre)
        {
            pPaquetre.Id = 0;

            //se asigna el token de autorización
            //client.DefaultRequestHeaders.Authorization = AutorizacionToken();


            var agregar = client.PostAsJsonAsync<Paquete>("/Paquetes/Agregar", pPaquetre);

            await agregar;  //se espera que termine la transacción

            //Emmm vemos  el resultado
            var resultado = agregar.Result;

            if (resultado.IsSuccessStatusCode) //si todo fue correcto
            {
          
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Mensaje"] = "No se logró registrar el paquete..";
                return View(pPaquetre);
            }

        }
        // GET: GestionarPaquetes/Details/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            var paquete = new Paquete();
            
            HttpResponseMessage response = await client.GetAsync($"/Paquetes/Buscar?id={id}");

           
            if (response.IsSuccessStatusCode)
            {
                
                var resultado = response.Content.ReadAsStringAsync().Result;

                
                paquete = JsonConvert.DeserializeObject<Paquete>(resultado);
            }

 
            return View(paquete);
        }


  
        // POST: GestionarPaquetes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind] Paquete pPaquete)
        {

            //Se agrega el token para la transacción

            //client.DefaultRequestHeaders.Authorization = AutorizacionToken();


        
            var modificar = client.PutAsJsonAsync<Paquete>("/Paquetes/Editar", pPaquete);
            await modificar; //Esperamos

            //se toma el resultado
            var resultado = modificar.Result;

            //se valida la respuesta
            if (resultado.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //se almacena un mensaje de error
                TempData["Mensaje"] = "Datos incorrectos..";

                if (resultado.StatusCode.ToString().Equals("Unauthorized"))
                {
                    return RedirectToAction("Login", "Usuarios");
                }
                else
                {
                    //se ubica al usuario dentro del formulario editar para que verifique los datos
                    return View(pPaquete);
              }

            }


        }

        // GET: GestionarPaquetes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _context.Paquetes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // POST: GestionarPaquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paquete = await _context.Paquetes.FindAsync(id);
            if (paquete != null)
            {
                _context.Paquetes.Remove(paquete);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var paquete = new Paquete();



            //Se utiliza la  API web para buscar el libro que deseamos editar los datos
            HttpResponseMessage response = await client.GetAsync($"/Paquetes/Buscar?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;

                paquete = JsonConvert.DeserializeObject<Paquete>(resultado);
            }

            return View(paquete);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeletePaqute")]
        public async Task<IActionResult> DeletePaqute(int id)
        {

            //Se agregan el token en la transacción
            //client.DefaultRequestHeaders.Authorization = AutorizacionToken();

     
            HttpResponseMessage response = await client.DeleteAsync($"/Paquetes/Eliminar?id={id}");

           
            if (response.StatusCode.ToString().Equals("Unauthorized"))
            {
                
                return RedirectToAction("Login", "Usuarios");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        private bool PaqueteExists(int id)
        {
            return _context.Paquetes.Any(e => e.Id == id);
        }
    }
}
