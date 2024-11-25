

namespace HotelesBeachProyecto.Models;

public class AutorizacionResponse
{
    public string Token { get; set; }
    public bool Resultado { get; set; }
    public string Msj { get; set; }
    public Usuario Usuario { get; set; }
}
