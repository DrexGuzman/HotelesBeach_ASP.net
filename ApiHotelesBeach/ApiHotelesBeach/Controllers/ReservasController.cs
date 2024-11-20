﻿using ApiHotelesBeach.Data;
using ApiHotelesBeach.Dto;
using ApiHotelesBeach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace ApiHotelesBeach.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservasController : Controller
    {
        private readonly DbContextHotel _context = null;

        public ReservasController(DbContextHotel pContext)
        {
            _context = pContext;
        }

        [HttpGet("Listado")]
        public List<Reserva> Listado()
        {
            List<Reserva> reservas = null;
            reservas = _context.Reservas
                .Include(a => a.Usuario)
                .Include(a => a.Paquete)
                .Include(a => a.FormaPago)
                .ToList();
            return reservas;
        }

        [HttpPost("Agregar")]
        public async Task<string> Agregar([FromBody] ReservaCreateDto reservaDto)
        {
            string mensaje = "Reserva creada correctamente";

            if (reservaDto == null)
            {
                return "La reservación no puede ser nula.";
            }

            var usuarioExiste = await _context.Usuarios.FirstOrDefaultAsync(u => u.Cedula == reservaDto.ClienteCedula);
            if (usuarioExiste == null)
            {
                return "No se encontró un usuario con esa cédula.";
            }

            var paqueteExiste = await _context.Paquetes.FindAsync(reservaDto.PaqueteId);
            if (paqueteExiste == null)
            {
                return "El paquete elegido no existe.";
            }

            var descuento = CalcularDescuento(reservaDto.CantidadNoches);
            var formaPago = CrearFormaPago(reservaDto);

            if (formaPago != null)
            {
                try
                {
                    _context.FormasPago.Add(formaPago);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            var montoTotal = (paqueteExiste.Costo * reservaDto.CantidadPersonas) * reservaDto.CantidadNoches;

            var montoDescuento = montoTotal - (montoTotal * descuento);

            var prima = montoDescuento * paqueteExiste.Prima;

            var pagoMes = (montoDescuento - prima) / paqueteExiste.Mensualidades;

            var montoTotalColones = await ConvertirAColones(montoTotal);

            var reserva = new Reserva
            {
                CantidadNoches = reservaDto.CantidadNoches,
                CantidadPersonas = reservaDto.CantidadPersonas,
                Descuento = (int)(descuento * 100),
                MontoRebajado = montoDescuento,
                MontoFinal = montoTotal,
                MontoFinalColones = montoTotalColones,
                Prima = prima,
                PagoMes = pagoMes,
                PaqueteId = reservaDto.PaqueteId,
                FormaPagoId = formaPago != null ? formaPago.Id : 1,
                ClienteCedula = reservaDto.ClienteCedula
            };

            try
            {
                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();
                return mensaje;
            }
            catch (Exception ex)
            {
                return $"Error al crear la reservación: {ex.Message}";
            }
        }

        private decimal CalcularDescuento(int cantidadNoches)
        {
            if (cantidadNoches >= 3 && cantidadNoches <= 6)
                return 0.10m;
            if (cantidadNoches >= 7 && cantidadNoches <= 9)
                return 0.15m;
            if (cantidadNoches >= 10 && cantidadNoches <= 12)
                return 0.20m;
            if (cantidadNoches > 13)
                return 0.25m;
            return 0.0m;
        }

        private async Task<decimal> ConvertirAColones(decimal montoEnDolares)
        {
            string apiUrl = "https://apis.gometa.org/tdc/tdc.json";
            decimal tipoCambio = 0;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        dynamic data = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                        if (data != null && data.venta != null)
                        {
                            tipoCambio = Convert.ToDecimal(data.venta.ToString(), CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            throw new Exception("El campo 'venta' no se encontró en la respuesta de la API.");
                        }
                    }
                    else
                    {
                        throw new Exception($"Error en la solicitud a la API: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consultar la API: {ex.Message}");
                    throw new Exception("No se pudo obtener el tipo de cambio.");
                }
            }

            if (tipoCambio == 0)
            {
                throw new Exception("No se pudo obtener el tipo de cambio.");
            }

            decimal montoEnColones = montoEnDolares * tipoCambio;

            return montoEnColones;
        }

        private FormaPago CrearFormaPago(ReservaCreateDto reservaDto)
        {
            return reservaDto.NombreFormaPago switch
            {
                "Tarjeta" => new FormaPago
                {
                    Nombre = reservaDto.NombreFormaPago,
                    Numero = reservaDto.Numero,
                    Banco = reservaDto.Banco,
                    CVV = reservaDto.CVV,
                    FechaExpiracion = reservaDto.FechaExpiracion,
                    NombreTitular = reservaDto.NombreTitular
                },
                "Cheque" => new FormaPago
                {
                    Nombre = reservaDto.NombreFormaPago,
                    Numero = reservaDto.Numero,
                    NombreTitular = reservaDto.NombreTitular
                },
                _ => null
            };
        }

        [HttpGet("Buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            Reserva reserva = _context.Reservas
                .Include(a => a.Usuario)
                .Include(a => a.Paquete)
                .Include(a => a.FormaPago)
                .FirstOrDefault(x => x.Id == id);

            if( reserva == null)
            {
                return NotFound($"No se ha encontrado una reserva con el id: {id}");
            }
            

            return Ok( reserva );
        }

        [HttpGet("BuscarPorUsuario/{cedula}")]
        public IActionResult BuscarPorUsuario(string cedula)
        {
            
            var reservas = _context.Reservas
                .Include(a => a.Usuario)
                .Include(a => a.Paquete)
                .Include(a => a.FormaPago)
                .Where(x => x.Usuario.Cedula == cedula)
                .ToList();

            if (reservas == null || reservas.Count == 0)
            {
                return NotFound($"No se encontraron reservas para el usuario con ID: {cedula}");
            }

            return Ok(reservas);
        }

        [HttpDelete("Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var reserva = _context.Reservas.FirstOrDefault(x => x.Id == id);

            if (reserva == null)
            {
                return NotFound(new { mensaje = $"No se ha encontrado una reserva con el ID: {id}" });
            }

            try
            {
                _context.Reservas.Remove(reserva);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al intentar eliminar la reserva.", detalle = ex.Message });
            }

            return Ok(new { mensaje = "Reserva eliminada exitosamente.", reservaEliminada = reserva });
        }

    }
}
