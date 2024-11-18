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
            //seeders de forma de pago
            modelBuilder.Entity<FormaPago>().HasData(
                new FormaPago
                {
                    Id = 1,
                    Nombre = "Efectivo"
                    //Numero = 0
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
            // Configurar la relación entre Reservacion y Usuario
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany() // Si Usuario tiene varias Reservaciones, usar `.WithMany(u => u.Reservaciones)`
                .HasForeignKey(r => r.ClienteCedula)
                .HasPrincipalKey(u => u.Cedula);

            // Configurar la relación entre Reservacion y Paquete
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Paquete)
                .WithMany() // Si Paquete tiene varias Reservaciones, usar `.WithMany(p => p.Reservaciones)`
                .HasForeignKey(r => r.PaqueteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar la relación entre Reservacion y FormaPago
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.FormaPago)
                .WithMany() // Si FormaPago tiene varias Reservaciones, usar `.WithMany(fp => fp.Reservaciones)`
                .HasForeignKey(r => r.FormaPagoId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
