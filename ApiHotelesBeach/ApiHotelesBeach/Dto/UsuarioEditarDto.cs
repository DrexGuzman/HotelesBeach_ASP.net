namespace ApiHotelesBeach.Dto
{
    public class UsuarioEditarDto
    {
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}
