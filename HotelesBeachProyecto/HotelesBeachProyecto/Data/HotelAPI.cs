namespace HotelesBeachProyecto.Data
{
    public class HotelAPI
    {

        public HttpClient Inicial() { 
        
            var client= new HttpClient();
            //client.BaseAddress = new Uri(""); cuando este deployado el api en sommee
            client.BaseAddress = new Uri("https://localhost:7060");
            return client;
        }
    }
}
