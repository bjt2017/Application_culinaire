using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public void Process(DataRow dr, DataSet ds, List<Accueil.ucIngredient.Ingredient> ingredients)
        {
            // Create a new PDF document
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            Document doc = new Document();
            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, new System.IO.FileStream(outFilePath, System.IO.FileMode.Create));
                document.Open();

                Paragraph title = new Paragraph(dr["description"].ToString());
                title.Alignment = Element.ALIGN_CENTER;
                title.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24f);
                document.Add(title);

                // Ajout d'un sous-titre
                Paragraph subtitle = new Paragraph("Une recette Cuisinatout");
                subtitle.Alignment = Element.ALIGN_CENTER;
                subtitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f);
                document.Add(subtitle);

                // Ajout d'une image
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("../../assets/recette/" + dr["description"].ToString() + ".png");
                image.Alignment = Element.ALIGN_CENTER;
                image.ScaleToFit(300f, 300f);
                document.Add(image);

                // Ajout d'un paragraphe
                Paragraph paragraph = new Paragraph("Ingrédients nécessaire: ");
                paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                document.Add(paragraph);

                // Ajout d'une liste
                List list = new List(List.UNORDERED);
                list.IndentationLeft = 30f;
                foreach (DataRow datarow in ds.Tables["IngrédientsRecette"].Select("codeRecette = " + dr["codeRecette"].ToString()))
                {
                    list.Add(ds.Tables["Ingrédients"].Select("codeIngredient = " + datarow["codeIngredient"].ToString())[0]["libIngredient"].ToString() + ": " + datarow["quantite"] + " " + datarow["unité"]);
                }

                document.Add(list);

                // Ajout d'un paragraphe
                paragraph = new Paragraph("\n");
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                document.Add(paragraph);
                    
                paragraph = new Paragraph("Préparation: ");
                paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                document.Add(paragraph);

                list = new List(List.UNORDERED);
                list.IndentationLeft = 30f;
                int nb = 1;
                foreach (DataRow datarow in ds.Tables["EtapesRecette"].Select("codeRecette = " + dr["codeRecette"].ToString()))
                {
                    list.Add(nb.ToString() + ". " + datarow["texteEtape"].ToString());
                    nb++;
                }

                document.Add(list);

                // Ajout d'un paragraphe
                paragraph = new Paragraph("\n");
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                document.Add(paragraph);

                // Ajout d'un paragraphe
                paragraph = new Paragraph("----------------------------------------------------------------------------------------------------");
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                document.Add(paragraph);

                if (ingredients.Count > 0)
                {
                    // Ajout d'un paragraphe
                    paragraph = new Paragraph("Votre liste de course: ");
                    paragraph.Alignment = Element.ALIGN_JUSTIFIED;
                    paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                    document.Add(paragraph);

                    // Ajout d'une liste
                    list = new List(List.UNORDERED);
                    list.IndentationLeft = 30f;
                    foreach (Accueil.ucIngredient.Ingredient ingredient in ingredients)
                    {
                        list.Add(ingredient.Name + ": " + ingredient.Quantiter + " " + ingredient.uniter);
                    }

                    document.Add(list);
                }

                // Ajout d'un paragraphe
                paragraph = new Paragraph("Bon appétit!");
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f);
                document.Add(paragraph);



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
        public void GenererListeCourse(DataSet ds, List<Accueil.ucIngredient.Ingredient> ingredients)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            Document doc = new Document();
            String name;
            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, new System.IO.FileStream(outFilePath, System.IO.FileMode.Create));
                document.Open();

                Paragraph title = new Paragraph("Liste de course");
                title.Alignment = Element.ALIGN_CENTER;
                title.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24f);
                document.Add(title);

                // Ajout d'un sous-titre
                Paragraph subtitle = new Paragraph("Une recette Cuisinatout");
                subtitle.Alignment = Element.ALIGN_CENTER;
                subtitle.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f);
                document.Add(subtitle);

                for (int i = 0; i < ds.Tables["Famille"].Rows.Count; i++)
                {

                    List list = new List(List.UNORDERED);
                    list.IndentationLeft = 30f;
                    foreach (Accueil.ucIngredient.Ingredient ingredient in ingredients)
                    {
                        name = ingredient.Name.Clone().ToString();
                        // Si la chaine contient un ' alors on le remplace par un '' pour que la requete SQL fonctionne
                        if (name.Contains("'"))
                        {
                            name = name.Replace("'", "''");
                        }
                        if (ds.Tables["Ingrédients"].Select("libIngredient = '" + name + "'")[0]["codeFamille"].ToString() == ds.Tables["Famille"].Rows[i]["codeFamille"].ToString())
                        {
                            list.Add(ingredient.Name + ": " + ingredient.Quantiter + " " + ingredient.uniter);
                        }
                    }
                    if (list.Items.Count > 0)
                    {
                        title = new Paragraph("Rayon " + ds.Tables["Famille"].Rows[i]["libFamille"].ToString());
                        title.Alignment = Element.ALIGN_LEFT;
                        title.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24f);
                        document.Add(title);
                    }
                    document.Add(list);
                }
                document.Close();
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
