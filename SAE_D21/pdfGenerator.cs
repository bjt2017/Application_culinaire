using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_D21
{
    internal class GenerateurPDF
    {
        String outFilePath;
        public GenerateurPDF(String outFilePath)
        {
            if (outFilePath == null)
            {
                throw new Exception("outFilePath cannot be null");
            }else if (outFilePath == "")
            {
                throw new Exception("outFilePath cannot be empty");
            }else if (!outFilePath.EndsWith(".pdf"))
            {
                outFilePath += ".pdf";
            }
            this.outFilePath = outFilePath;
        }

        public void Process(DataRow dr)
        {
            // Create a new PDF document
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            Document doc = new Document();
            System.Console.WriteLine(this.outFilePath);
            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, new System.IO.FileStream(outFilePath, System.IO.FileMode.Create));
                document.Open();
                Paragraph title = new Paragraph(dr["description"].ToString());
                title.Alignment = Element.ALIGN_CENTER;
                title.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24f);
                document.Add(title);

                // Ajout d'un sous-titre
                Paragraph subtitle = new Paragraph("Sous-titre du document");
                subtitle.Alignment = Element.ALIGN_CENTER;
                subtitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f);
                document.Add(subtitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Close the document
                document.Close();
            }
        }
    }
}
