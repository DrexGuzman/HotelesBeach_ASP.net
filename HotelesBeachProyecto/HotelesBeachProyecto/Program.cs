using Microsoft.EntityFrameworkCore;
using HotelesBeachProyecto.Data; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DbContextHotel>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication",
    config => {
        config.Cookie.Name = "UserloginCookie";
        config.Cookie.HttpOnly = true;
        config.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        config.LoginPath = "/Usuarios/Login";
        config.AccessDeniedPath = "/Usuarios/AccessDenied";
        config.SlidingExpiration = true;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
