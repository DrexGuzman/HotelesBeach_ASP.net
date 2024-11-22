using Azure;
using HotelesBeachProyecto.Data;
using HotelesBeachProyecto.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace HotelesBeachProyecto.Controllers
{
    public class GestionarUsuariosController : Controller
    {

        private HotelAPI hotelAPI;
        private HttpClient client;

        public GestionarUsuariosController()
        {

            hotelAPI = new HotelAPI();
            client = hotelAPI.Inicial();
        }


        public async Task<IActionResult> Index()
        {

            List<Usuario> usuarios = new List<Usuario>();
            //se utiliza el metodo de la api

            HttpResponseMessage response = await client.GetAsync("/Usuarios/Listado");

            if (response.StatusCode.ToString().Equals("Unauthorized"))
            {

                return RedirectToAction("Login", "Usuarios");
            }

            if (response.IsSuccessStatusCode)
            {
                var resultado = response.Content.ReadAsStringAsync().Result;

                usuarios = JsonConvert.DeserializeObject<List<Usuario>>(resultado);
            }

            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioAct pUsuario, int cedula)
        {

            if(cedula == 0)
            {
                return RedirectToAction("Index");
            }

            //Se agregan el token en la transacción
            client.DefaultRequestHeaders.Authorization = AutorizacionToken();


            var modificar = client.PutAsJsonAsync<UsuarioAct>($"/Usuarios/Actualizar/{cedula}", pUsuario); 
           
            await modificar; //Esperamos



            //se toma el resultado
            var resultado = modificar.Result;

            if (resultado.StatusCode.ToString().Equals("Unauthorized"))
            {

                return RedirectToAction("Login", "Usuarios");
            }

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
                   
                    return RedirectToAction("Index");
                }

            }


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
