using ApiHotelesBeach.Data;
using ApiHotelesBeach.Dto;
using ApiHotelesBeach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            decimal descuento = 0.0m;
            if (reservaDto.CantidadNoches <= 3 && reservaDto.CantidadNoches >= 6)
            {
                descuento = 0.10m;
            }
            else if (reservaDto.CantidadNoches <= 7 && reservaDto.CantidadNoches >= 9)
            {
                descuento = 0.15m;
            }
            else if (reservaDto.CantidadNoches <= 10 && reservaDto.CantidadNoches >= 12)
            {
                descuento = 0.20m;
            }
            else if (reservaDto.CantidadNoches <= 13)
            {
                descuento = 0.25m;
            };
            FormaPago formaPago = null;

            switch (reservaDto.NombreFormaPago)
            {
                case "Tarjeta":
                    formaPago = new FormaPago
                    {
                        Nombre = reservaDto.NombreFormaPago,
                        Numero = reservaDto.Numero,
                        Banco = reservaDto.Banco,
                        CVV = reservaDto.CVV,
                        FechaExpiracion = reservaDto.FechaExpiracion,
                        NombreTitular = reservaDto.NombreTitular
                    };
                    break;

                case "Cheque":
                    formaPago = new FormaPago
                    {
                        Nombre = reservaDto.NombreFormaPago,
                        Numero = reservaDto.Numero,
                        NombreTitular = reservaDto.NombreTitular
                    };
                    break;

                default:
                    break;
            }

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

            var reserva = new Reserva
            {
                CantidadNoches = reservaDto.CantidadNoches,
                CantidadPersonas = reservaDto.CantidadPersonas,
                Descuento = descuento,
                MontoRebajado = montoDescuento,
                MontoFinal = montoTotal,
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
    }
}
