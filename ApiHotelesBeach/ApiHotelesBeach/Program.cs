using ApiHotelesBeach;
using ApiHotelesBeach.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración desde secrets.json
builder.Configuration.AddUserSecrets<Program>();

// Configuración del servicio de Email
builder.Services.AddTransient<IServicioEmail, ServicioEmail>();

// Add services to the container.
builder.Services.AddControllers();

// Construir el string de conexión desde secrets.json
var connectionString = new SqlConnectionStringBuilder
{
    DataSource = builder.Configuration.GetValue<string>("CONFIGURACIONES_STRING:SERVER"),
    InitialCatalog = builder.Configuration.GetValue<string>("CONFIGURACIONES_STRING:DATABASE"),
    UserID = builder.Configuration.GetValue<string>("CONFIGURACIONES_STRING:USER"),
    Password = builder.Configuration.GetValue<string>("CONFIGURACIONES_STRING:PASSWORD"),
    TrustServerCertificate = builder.Configuration.GetValue<bool>("CONFIGURACIONES_STRING:TrustServerCertificate")
}.ConnectionString;

// Registrar el DbContext usando el string de conexión construido
builder.Services.AddDbContext<ApiHotelesBeach.Data.DbContextHotel>(options =>
    options.UseSqlServer(connectionString));

// Configurar servicios personalizados
builder.Services.AddScoped<IAutorizacionServices, AutorizacionServices>();
builder.Services.AddScoped<ServicioEmail>();
builder.Services.AddScoped<InvoiceService>();

// Configurar servicio de JWT
var jwtKey = builder.Configuration.GetValue<string>("CONFIGURACIONES_JWT:KEY");
var jwtKeyBytes = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
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

// Configuración de autenticación JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();