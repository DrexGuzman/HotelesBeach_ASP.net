using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace ApiHotelesBeach.Services
{
    public class InvoiceService
    {

        public PdfDocument GetInvoice()
        {
            // Create a document
            var document = new Document();

            // Render the document to a PDF file
            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

    }
}
