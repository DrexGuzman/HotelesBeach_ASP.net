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

            reservaInfo.AddText($"Paquete: {reserva.Paquete.Nombre}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Costo Paquete: {reserva.Paquete.Costo.ToString("C", new System.Globalization.CultureInfo("en-US"))} por persona");
            reservaInfo.AddLineBreak();

            // Calcular el descuento y monto rebajado
            decimal descuentoAplicado = reserva.Descuento / 100m; // Descuento en formato decimal (0.10 -> 10%)
            decimal montoRebajado = reserva.Paquete.Costo * reserva.CantidadPersonas * descuentoAplicado;
            decimal montoFinal = (reserva.Paquete.Costo * reserva.CantidadPersonas) - montoRebajado;

            reservaInfo.AddText($"Descuento Aplicado: {descuentoAplicado * 100:0}%"); // Eliminar decimales extras
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Monto Rebajado: {reserva.MontoRebajado.ToString("C", new System.Globalization.CultureInfo("en-US"))}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Monto Final Dólares: {reserva.MontoFinal.ToString("C", new System.Globalization.CultureInfo("en-US"))}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddText($"Monto Final Colones: {reserva.MontoFinalColones:C}");
            reservaInfo.AddLineBreak();
            reservaInfo.AddLineBreak();

            // Información del método de pago
            var pagoInfo = section.AddParagraph();
            pagoInfo.AddFormattedText("Método de Pago", TextFormat.Bold);
            pagoInfo.AddLineBreak();

            // Verificar si FormaPago es null
            if (reserva.FormaPago != null)
            {
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
            }
            else
            {
                pagoInfo.AddText("Método de pago no especificado.");
            }

            pagoInfo.AddLineBreak();
        }
    }
}
