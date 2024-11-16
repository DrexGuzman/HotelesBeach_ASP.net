namespace HotelesBeachProyecto.Models
{
    public class Reservacion
    {

        // Información del cliente
        public string? TipoIdentificacion { get; set; }
        public string? Identificacion { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Telefono { get; set; }
        public string? CorreoElectronico { get; set; }

        // Información de la reserva
        public string? PaqueteSeleccionado { get; set; }
        public int CantidadNoches { get; set; }
        public int CantidadPersonas { get; set; }
        public decimal Descuento { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal Prima { get; set; }
        public int CuotasPendientes { get; set; }
        public decimal MontoCuota { get; set; }

    }
}
