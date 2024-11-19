// Referencia para los modelos
using ApiHotelesBeach.Models;

// Referencia para utilizar el AutorizacionResponse
using ApiHotelesBeach.Models.Custom;

// Referencia para utilizar el ORM
using ApiHotelesBeach.Data;

// Referencia para utilizar las librería del ORM
using Microsoft.EntityFrameworkCore;

// Referencia para librerías de JWT
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiHotelesBeach.Services
{
    public class AutorizacionServices : IAutorizacionServices
    {
        // Variable para utilizar el archivo appsettings.json
        private readonly IConfiguration _configuration;

        // Variable para utilizar el DbContext con sus métodos ORM
        private readonly DbContextHotel _context;

        public AutorizacionServices(IConfiguration configuration, DbContextHotel dbContext)
        {
            _configuration = configuration;
            _context = dbContext;
        }
        public async Task<AutorizacionResponse> DevolverToken(Usuario autorizacion)
        {
            // Se identifica al ususario que está solicitando la autorización
            var temp = await _context.Usuarios.FirstOrDefaultAsync(u=>u.Email.Equals(autorizacion.Email) && u.Password.Equals(autorizacion.Password));

            if (temp == null)
            {
                return await Task.FromResult < AutorizacionResponse>(null);
            }
            else
            {
                // Generar el token
                string tokenCreado = GenerarToken(autorizacion.Email.ToString());

                // Retornarlo
                return new AutorizacionResponse() {  Token = tokenCreado, Resultado = true, Msj="Ok" };
            }
        }

        public string GenerarToken(string IDUsuario)
        {
            // Se realiza la lectura de la key almacenad dentro del archivo de configuración JSON
            var key = _configuration.GetValue<string>("JwtSettings:key");

            // Se convierte la key en un vector de bytes
            var KeyBytes = Encoding.ASCII.GetBytes(key);

            // Se declara la identidad que realiza el reclamo para la solicitud de autorización
            var claims = new ClaimsIdentity();

            // Se asigna su identificador
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IDUsuario));

            // Se instancian las credenciales del token
            var credencialesToken = new SigningCredentials(new SymmetricSecurityKey(KeyBytes),
                SecurityAlgorithms.HmacSha256Signature); // Algoritmo de cifrado

            // Se instancia el descriptor para el token
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(30), // El token expirará después de 10 minutos
                SigningCredentials = credencialesToken // Se asignan las credenciales
            };

            // Instanciar el tokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Crear el token
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            // Escribir el token
            var tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }
    }// Cierre clase
}// Cierre namespace
