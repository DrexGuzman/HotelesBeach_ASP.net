using ApiHotelesBeach.Data;
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

            var formaPagoExiste = await _context.FormasPago.FindAsync(reservaDto.FormaPagoId);
            if (formaPagoExiste == null)
            {
                return "La forma de pago indicada no existe.";
            }

            var reserva = new Reserva
            {
                CantidadNoches = reservaDto.CantidadNoches,
                CantidadPersonas = reservaDto.CantidadPersonas,
                Descuento = reservaDto.Descuento,
                MontoRebajado = reservaDto.MontoRebajado,
                MontoFinal = reservaDto.MontoFinal,
                PaqueteId = reservaDto.PaqueteId,
                FormaPagoId = reservaDto.FormaPagoId,
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
    }
}
