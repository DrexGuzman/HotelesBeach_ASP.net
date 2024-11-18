﻿using ApiHotelesBeach.Data;
using ApiHotelesBeach.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<string> Agregar (Paquete paquete)
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
    }
}
