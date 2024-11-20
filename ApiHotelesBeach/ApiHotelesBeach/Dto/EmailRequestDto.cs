namespace ApiHotelesBeach.Dto
{
    public class EmailRequestDto
    {
        public string EmailReceptor { get; set; }
        public string Tema { get; set; }
        public string Cuerpo { get; set; }
        public List<AdjuntoDto>? ArchivosAdjuntos { get; set; } = new List<AdjuntoDto>();
    }
}
