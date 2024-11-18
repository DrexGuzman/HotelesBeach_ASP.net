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

    }
}
