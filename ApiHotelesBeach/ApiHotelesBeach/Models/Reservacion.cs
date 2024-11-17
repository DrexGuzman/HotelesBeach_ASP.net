﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiHotelesBeach.Models
{
    public class Reservacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 365)]
        public int CantidadNoches { get; set; }

        [Required]
        [Range(1, 8)]
        public int CantidadPersonas { get; set; }

        [Required(ErrorMessage = "Debe indicar el descuento a realizar en la reserva")]
        public decimal Descuento { get; set; }

        [Required(ErrorMessage = "Debe indicar el monto de la reserva sin el descuento y con el IVA aplicado")]
        public decimal MontoRebajado { get; set; }

        [Required(ErrorMessage = "Debe indicar el monto final total de la reserva")]
        public decimal MontoFinal { get; set; }

        [Required(ErrorMessage = "Debe indicar el paquete elegido")]
        public int PaqueteId { get; set; }
        public Paquete Paquete { get; set; }

        [Required(ErrorMessage = "Debe indicar la forma de pago")]
        public int FormaPagoId { get; set; }
        public FormaPago FormaPago { get; set; }

        [Required(ErrorMessage = "Debe indicar el cliente que hace la reservación")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
