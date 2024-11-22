using ApiHotelesBeach.Models;
using ApiHotelesBeach.Models.Custom;


namespace ApiHotelesBeach.Services
{
    public interface IAutorizacionServices
    {
        Task<AutorizacionResponse> DevolverToken(Usuario autorizacion);
    }
}
