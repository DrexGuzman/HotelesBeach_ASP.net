using ApiHotelesBeach.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApiHotelesBeach.Data.DbContextHotel>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("StringConexion")));

// Configurar servicio de JWT
builder.Services.AddScoped<IAutorizacionServices, AutorizacionServices>();

// Configurar el servicio de PDF
builder.Services.AddScoped<InvoiceService>();

// Se toma la llave a utilizar para generar el token
var key = builder.Configuration.GetValue<string>("JwtSettings:Key");
var keyBytes = Encoding.ASCII.GetBytes(key);

// Se configura el esquema de autenticación
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true, // Validar la key para el inicio de sesión
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false, // No se valida el emisor
        ValidateAudience = false, // No se valida la audiencia
        ValidateLifetime = true, // No se valida la vida del token
        ClockSkew = TimeSpan.Zero, // No debe existir diferencia del tiempo del reloj
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configuración de authenticación JWT
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
