using Microsoft.EntityFrameworkCore;
using ApiHotelesBeach.Models;

namespace ApiHotelesBeach.Data
{
    public class DbContextHotel : DbContext
    {
        public DbContextHotel(DbContextOptions<DbContextHotel> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<FormaPago> FormasPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Se crea la tabla libro y se le almacena un registro
            modelBuilder.Entity<FormaPago>().HasData(
                new FormaPago
                {
                    Id = 1,
                    Nombre = "Efectivo"
                    //Numero = 0 // No aplica para efectivo, pero requiere un valor por defecto
                    //Banco = null,
                    //CVV = null,
                    //FechaExpiracion = null,
                    //NombreTitular = null
                },
                new FormaPago
                {
                    Id = 2,
                    Nombre = "Tarjeta de Crédito",
                    Numero = 1010101,
                    Banco = "Banco Nacional",
                    CVV = "123",
                    FechaExpiracion = "12/25",
                    NombreTitular = "Juan Pérez"
                }
                );

        }

    }
}
