using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using ApiHotelesBeach.Models;

namespace ApiHotelesBeach.Services
{
    public class InvoiceService
    {
        public PdfDocument GetInvoice(Reserva reserva)
        {
            // Crear un documento
            var document = new Document();
            BuildDocument(document, reserva);

            // Renderizar el documento a un PDF
            var pdfRenderer = new PdfDocumentRenderer
            {
                Document = document
            };

            pdfRenderer.RenderDocument();
            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document, Reserva reserva)
        {
            // Sección principal
            Section section = document.AddSection();

            // Encabezado
            var header = section.AddParagraph();
            header.AddFormattedText("Hoteles Beach S.A.", TextFormat.Bold);
            header.Format.Font.Size = 16;
            header.Format.Alignment = ParagraphAlignment.Center;
            header.AddLineBreak();

            header.AddText($"Reserva ID: {reserva.Id}");
            header.AddLineBreak();
            header.AddLineBreak();

            // Información del cliente
            var clienteInfo = section.AddParagraph();
            clienteInfo.AddFormattedText("Información del Cliente", TextFormat.Bold);
            clienteInfo.AddLineBreak();
            clienteInfo.AddText($"Nombre: {reserva.Usuario.NombreCompleto}");
            clienteInfo.AddLineBreak();
            clienteInfo.AddText($"Cédula: {reserva.ClienteCedula}");
            clienteInfo.AddLineBreak();
            clienteInfo.AddText($"Teléfono: {reserva.Usuario.Telefono}");
            clienteInfo.AddLineBreak();
            clienteInfo.AddText($"Dirección: {reserva.Usuario.Direccion}");
            clienteInfo.AddLineBreak();
            clienteInfo.AddLineBreak();

            // Información de la reserva
            var reservaInfo = section.AddParagraph();
            reservaInfo.AddFormattedText("Detalles de la Reserva", TextFormat.Bold);
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Cantidad de Noches: {reserva.CantidadNoches}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Cantidad de Personas: {reserva.CantidadPersonas}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Descuento Aplicado: {reserva.Descuento * 100}%");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Monto Rebajado: {reserva.MontoRebajado:C}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Monto Final: {reserva.MontoFinal:C}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddLineBreak();

            // Información del método de pago
            var pagoInfo = section.AddParagraph();
            pagoInfo.AddFormattedText("Método de Pago", TextFormat.Bold);
            pagoInfo.AddLineBreak();
            pagoInfo.AddText($"Forma de Pago: {reserva.FormaPago.Nombre}");
            pagoInfo.AddLineBreak();

            if (reserva.FormaPago.Numero.HasValue)
            {
                pagoInfo.AddText($"Número: {reserva.FormaPago.Numero}");
                pagoInfo.AddLineBreak();
            }

            if (!string.IsNullOrEmpty(reserva.FormaPago.Banco))
            {
                pagoInfo.AddText($"Banco: {reserva.FormaPago.Banco}");
                pagoInfo.AddLineBreak();
            }

            if (!string.IsNullOrEmpty(reserva.FormaPago.NombreTitular))
            {
                pagoInfo.AddText($"Titular: {reserva.FormaPago.NombreTitular}");
                pagoInfo.AddLineBreak();
            }

            pagoInfo.AddLineBreak();
        }
    }
}
