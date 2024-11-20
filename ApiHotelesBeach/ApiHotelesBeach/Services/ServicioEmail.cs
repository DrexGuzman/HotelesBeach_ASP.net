using ApiHotelesBeach.Dto;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ApiHotelesBeach.Services
{
    public interface IServicioEmail
    {
        Task EnviarEmail(string emailReceptor, string tema, string cuerpo);
        Task EnviarEmailConAdjuntos(EmailRequestDto emailRequest);
    }

    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration configuration;

        // Variables globales de configuración
        private readonly string emailEmisor;
        private readonly string password;
        private readonly string host;
        private readonly int puerto;

        public ServicioEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
            emailEmisor = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:EMAIL");
            password = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:PASSWORD");
            host = configuration.GetValue<string>("CONFIGURACIONES_EMAIL:HOST");
            puerto = configuration.GetValue<int>("CONFIGURACIONES_EMAIL:PUERTO");
        }

        public async Task EnviarEmail(string emailReceptor, string tema, string cuerpo)
        {
            var smtpCliente = new SmtpClient(host, puerto)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailEmisor, password) 
            };

            var mensaje = new MailMessage(emailEmisor, emailReceptor, tema, cuerpo);
            await smtpCliente.SendMailAsync(mensaje);
        }

        public async Task EnviarEmailConAdjuntos(EmailRequestDto emailRequest)
        {
            if (emailRequest == null)
                throw new ArgumentNullException(nameof(emailRequest));

            if (string.IsNullOrEmpty(emailRequest.EmailReceptor))
                throw new ArgumentException("El correo electrónico del receptor es obligatorio", nameof(emailRequest.EmailReceptor));

            if (string.IsNullOrEmpty(emailRequest.Tema))
                throw new ArgumentException("El tema es obligatorio", nameof(emailRequest.Tema));

            if (string.IsNullOrEmpty(emailRequest.Cuerpo))
                throw new ArgumentException("El cuerpo del mensaje es obligatorio", nameof(emailRequest.Cuerpo));

            using (var smtpClient = new SmtpClient(host, puerto))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailEmisor, password); 

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailEmisor),
                    Subject = emailRequest.Tema,
                    Body = emailRequest.Cuerpo,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailRequest.EmailReceptor);

                foreach (var archivo in emailRequest.ArchivosAdjuntos)
                {
                    if (archivo == null || archivo.Contenido == null || archivo.Contenido.Length == 0)
                        throw new ArgumentException("El archivo adjunto no tiene contenido válido", nameof(archivo));

                    var attachment = new Attachment(new MemoryStream(archivo.Contenido), archivo.NombreArchivo);
                    mailMessage.Attachments.Add(attachment);
                }

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
