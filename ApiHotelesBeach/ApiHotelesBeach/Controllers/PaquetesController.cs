using ApiHotelesBeach.Data;
using ApiHotelesBeach.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

namespace ApiHotelesBeach.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaquetesController : Controller
    {
        private readonly DbContextHotel _context = null;

        public PaquetesController(DbContextHotel pContext)
        {
            _context = pContext;
        }

        [HttpGet("Listado")]
        public List<Paquete> Listado()
        {
            List<Paquete> paquetes = null;
            paquetes = _context.Paquetes.ToList();
            return paquetes;
        }

        [HttpPost("Agregar")]
        public async Task<string> Agregar(Paquete paquete)
        {
            string mensaje = "";
            if (paquete == null)
            {
                mensaje = "Debe de ingresar los datos de un paquete";
                return mensaje;
            }
            try
            {
                _context.Paquetes.Add(paquete);
                await _context.SaveChangesAsync();
                mensaje = "Paquete agregado con exito";
                return mensaje;
            }
            catch (Exception ex)
            {

                mensaje = $"Error al guardar el paquete: {ex.Message}";
                return mensaje;
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<string> Eliminar(int id)
        {
            string mensaje = $"Paquete no eliminado, {id} no existe";

            Paquete temp = _context.Paquetes.FirstOrDefault(x => x.Id == id);

            if (temp != null)
            {
                _context.Paquetes.Remove(temp);
                await _context.SaveChangesAsync();
                mensaje = $"Paquete {temp.Nombre} eliminado correctamente";
            }

            return mensaje;
        }

        [HttpPut("Editar")]
        public async Task<string> Editar(Paquete temp)
        {
            var paquete = _context.Paquetes.FirstOrDefault(x => x.Id == temp.Id);
            string mensaje = "";
            if (paquete == null)
            {
                mensaje = $"El paquete {temp.Nombre} no existe";
                return mensaje;
            }

            try
            {
                paquete.Nombre = temp.Nombre;
                paquete.Costo = temp.Costo;
                paquete.Prima = temp.Prima;
                paquete.Mensualidades = temp.Mensualidades;

                _context.Paquetes.Update(paquete);
                await _context.SaveChangesAsync();
                mensaje = $"Paquete {paquete.Nombre} actualizado correctamente";
                return mensaje;
            }
            catch (Exception ex)
            {

                mensaje = $"Error al actualizar el Paquete: {ex.Message}";
                return mensaje;
            }
        }

        [HttpGet("Buscar")]
        public Paquete Buscar(int id)
        {
            Paquete paquete = _context.Paquetes.FirstOrDefault(u => u.Id == id);

            return paquete == null ? new Paquete { Nombre = "No existe" } : paquete;
        }
    }
}
