using System.Net.Mail;
using System.Net;

namespace ApiHotelesBeach.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, byte[] pdfBytes)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("hotelbeachsa506@gmail.com", "mtzk zcuf hjbs pjaj")
            };

            // Crear el mensaje de correo
            var mailMessage = new MailMessage(
                from: "hotelbeachsa506@gmail.com",
                to: email,
                subject: subject,
                body: message
            );

            // Agregar el archivo PDF como adjunto
            using (var stream = new MemoryStream(pdfBytes))
            {
                var attachment = new Attachment(stream, "Reserva.pdf", "application/pdf");
                mailMessage.Attachments.Add(attachment);
            }

            // Enviar el correo
            return client.SendMailAsync(mailMessage);
        }
    }
}
