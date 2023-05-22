using Accueil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accueil;

namespace SAE_D21
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.loadDataset();
            this.loadmenu();
            folderBrowserDialog.SelectedPath = "C:\\Users\\arnaudmichel\\source\\repos\\SAE_D21\\SAE_D21\\pdfRecettes";
           

        }

        Random rnd = new Random();

        private void loadmenu() {
            Accueil.BarDeRecherche barDeRecherche1 = new Accueil.BarDeRecherche();
            barDeRecherche1.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barDeRecherche1_KeyPress);
            barDeRecherche1.Location = new Point((this.Width - barDeRecherche1.Width)/2, 87);


            ucBarre1.SetClick(this.Click_Recherche_Ingredient);

            Label Titre = new Label();
            
            Titre.Size = new System.Drawing.Size(500, 42);
            Titre.Text = "QU'EST CE QU'ON MANGE CE SOIR ?";
            
            Titre.Font = new System.Drawing.Font("Bahnschrift", 18, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Titre.TextAlign = ContentAlignment.MiddleCenter;
            Titre.Location = new Point((this.Width) / 2 - (Titre.Size.Width)/2,30);

            

            Label Titre2 = new Label();
            Titre2.Size = new System.Drawing.Size(300, 42);
            Titre2.Text = "Nos recommandations";
            
            Titre2.Font = new System.Drawing.Font("Bahnschrift", 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Titre2.Location = new Point((this.Width) / 2 - (Titre2.Width)/2, 140);
            Titre2.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(Titre); this.Controls.Add(Titre2);


            this.Controls.Add(barDeRecherche1);
            DataTable dtUnselected = dataset.Tables["Recettes"].Copy();
            for (int i = 0; i < 2; i++)
            {
                int decale_y = 0;
                for (int j = 0; j < 3; j++)
                {
                    int decale_x = 0; 
                    if (j > 0)
                    {
                        decale_x = j * 20;
                    }
                    if (i > 0)
                    {
                        decale_y = i * 5;
                    }
                    int row = rnd.Next(dtUnselected.Rows.Count);
                    DataRow dr = dtUnselected.Rows[row].Table.NewRow();
                    dr.ItemArray = dtUnselected.Rows[row].ItemArray.Clone() as object[];
                    ucCarte ucCarte = new ucCarte(dr);

                    try
                    {
                        dtUnselected.Rows.RemoveAt(row);
                    }catch(Exception ex)
                    {

                    }
                    ucCarte.Location = new Point(25 + j * ucCarte.Width + decale_x, 420 + i * ucCarte.Height + decale_y);
                    ucCarte.setClick(carteGrande_Click);
                    this.Controls.Add(ucCarte);
                }
                
            }
            for (int i = 0; i < 5; i++)
            {
                int decale_x = 0;
                if (i > 0)
                {
                    decale_x = i * 170;
                }
                int row = rnd.Next(dtUnselected.Rows.Count);
                DataRow dr = dtUnselected.Rows[row].Table.NewRow();
                dr.ItemArray = dtUnselected.Rows[row].ItemArray.Clone() as object[];
                Accueil.carteGrande carteGrande = createCarteGrande(dr, this.Width / 2 - 425 + decale_x, 200);
                try
                {
                    dtUnselected.Rows.RemoveAt(row);
                }
                catch (Exception ex)
                {

                }
                this.Controls.Add(carteGrande);
            }
            
        }

        // Connection string to database file
        string chcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=baseFrigo.accdb";
        OleDbConnection con = new OleDbConnection();
        DataSet dataset = new DataSet();


        //Variable, tableau
        string[] ingredients = new string[3];
        DataRow[] rowingredient = new DataRow[3];
        List<DataRow> recettes = new List<DataRow>();

        // Load dataset
        private void loadDataset()
        {
            con.ConnectionString = chcon;
            con.Open();
            // Get all sheets
            DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            // Get all data from sheets
            foreach (DataRow row in dt.Rows)
            {
                // Get sheet name
                string sheet = row["TABLE_NAME"].ToString();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + sheet + "]", con);
                // Fill dataset with sheet data
                try
                {
                    adapter.Fill(dataset, sheet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        
        private ucCarte createCarte(DataRow dr, int x, int y)
        {
            // Create carte
            ucCarte carte = new ucCarte(dr);
            carte.Location = new Point(x, y);
            carte.setClick(carteGrande_Click);
            return carte;
        }

        private carteGrande createCarteGrande(DataRow dr, int x, int y)
        {
            // Create carte
            carteGrande carte = new carteGrande(dr);
            carte.Location = new Point(x, y);
            carte.setClick(carteGrande_Click);
            return carte;
        }
        private ucCarteEtoile createCarteStars(DataRow dr, int x, int y)
        {
            // Create carte
            ucCarteEtoile carte = new ucCarteEtoile(dr);
            carte.Location = new Point(x, y);
            carte.setClick(carteGrande_Click);
            return carte;
        }

        private void clearctrl()
        {
            bool control = true;
            while (control)
            {
                control = false;
                foreach (Control c in this.Controls)
                {
                    if (c is ucCarte || c is ucCarteEtoile || c is carteGrande)
                    {
                        this.Controls.Remove(c);
                        control = true;
                    }
                }
            }
        }

        private void rechercher(System.Windows.Forms.TextBox searchbar)
        {
            // Remov all the uccarte on the screen
            recettes.Clear();
            ingredients = new string[3];
            rowingredient = new DataRow[3];

            this.clearctrl();


            String Texte = searchbar.Text.Trim().ToLower().Replace(", ", ",");
            ingredients = Texte.Split(',');

            if (ingredients.Length > 3)
            {

                errorProvider.SetError(searchbar.Parent.Parent, "Vous ne pouvez pas entrer plus de 3 ingrédients");
                searchbar.Parent.Parent.BackColor = Color.IndianRed;
                searchbar.ForeColor = Color.IndianRed;
                ingredients = new string[3];
                return;

            }
            else
            {
                errorProvider.Clear();
                for (int i = 0; i < ingredients.Length; i++)
                {
                    if (dataset.Tables["Ingrédients"].Select("libIngredient = '" + ingredients[i] + "'").Length == 0)
                    {
                        searchbar.Parent.Parent.BackColor = Color.IndianRed;
                        searchbar.ForeColor = Color.IndianRed;
                        errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;

                        errorProvider.SetIconPadding(searchbar.Parent.Parent, 5);
                        errorProvider.SetError(searchbar.Parent.Parent, "L'ingrédient " + ingredients[i] + " n'existe pas");
                        ingredients = new string[3];
                        return;
                    }
                    else
                    {
                        errorProvider.Clear();
                        rowingredient[i] = dataset.Tables["Ingrédients"].Select("libIngredient = '" + ingredients[i] + "'")[0];
                    }
                }
                this.listeIngredientInRecette();
            }
        }

        private void barDeRecherche1_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Windows.Forms.TextBox searchbar = (System.Windows.Forms.TextBox)sender;
            searchbar.Parent.Parent.BackColor = Color.DarkGray;
            searchbar.ForeColor = Color.DimGray;
            errorProvider.Clear();
            
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.rechercher(searchbar);
            }else if (e.KeyChar == (char)Keys.Back && searchbar.Text.Length <= 1)
            {
                this.clearctrl();
                this.loadmenu();
            }
            else if (e.KeyChar == (char)Keys.X)
            {
                folderBrowserDialog.ShowDialog();
                if (folderBrowserDialog.SelectedPath != "")
                {
                    GenerateurPDF pdf = new GenerateurPDF(folderBrowserDialog.SelectedPath + "\\Marecette.pdf");
                    pdf.Process(dataset.Tables["recettes"].Rows[0]);
                }
            }
        }
        private void listeIngredientInRecette()
        {
            List<DataRow> ingrédientsrecette = new List<DataRow>();
            foreach (DataRow row in rowingredient)
            {
                try
                {
                    if (row != null && dataset.Tables["IngrédientsRecette"].Select("codeIngredient = '" + row["codeIngredient"] + "'").Length == 0)
                    {
                        //errorProvider.SetError(sear.textBox, "Aucune recette n'est faite de " + row["libIngredient"]);
                        ingredients = new string[3];
                        return;
                    }
                    else
                    {
                        errorProvider.Clear();
                        if (row != null)
                        {
                            DataRow[] dr = dataset.Tables["IngrédientsRecette"].Select("codeIngredient = '" + row["codeIngredient"] + "'");
                            foreach (DataRow r in dr)
                            {
                                if (!recettes.Contains(dataset.Tables["Recettes"].Select("codeRecette = '" + r["codeRecette"] + "'")[0]))
                                {
                                    recettes.Add(dataset.Tables["Recettes"].Select("codeRecette = '" + r["codeRecette"] + "'")[0]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            for (int i = 0; i < recettes.Count; i++)
            {
                DataRow row = recettes[i];
                ucCarteEtoile carte = this.createCarteStars(row, 10, (i * 93));
                this.Controls.Add(carte);
            }
        }

        private void carteGrande_Click(object sender, EventArgs e)
        {
            DataRow row = dataset.Tables["Recettes"].Rows[0];
            
            if (sender is Panel)
            {
                if(((Panel)sender).Parent is ucCarte)
                {
                    row = ((ucCarte)((Panel)sender).Parent).drow;
                }
                else if (((Panel)sender).Parent is ucCarteEtoile)
                {
                    row = ((ucCarteEtoile)((Panel)sender).Parent).drow;
                }
                else if (((Panel)sender).Parent is carteGrande)
                {
                    row = ((carteGrande)((Panel)sender).Parent).drow;
                }
            }
            else if (sender is UserControl) {
                row = ((carteGrande)sender).drow;
            }
            else if(sender is PictureBox)
            {
                if (((PictureBox)sender).Parent.Parent is ucCarte)
                {
                    row = ((ucCarte)((PictureBox)sender).Parent.Parent).drow;
                }
                else if (((PictureBox)sender).Parent.Parent is ucCarteEtoile)
                {
                    row = ((ucCarteEtoile)((PictureBox)sender).Parent.Parent).drow;
                }
                
            }
            else if (sender is Label) {
                
                if (((Label)sender).Parent.Parent is ucCarte)
                {
                    row = ((ucCarte)((Label)sender).Parent.Parent).drow;
                }
                else if (((Label)sender).Parent.Parent is ucCarteEtoile)
                {
                    row = ((ucCarteEtoile)((Label)sender).Parent.Parent).drow;
                }
                else if (((Label)sender).Parent.Parent is carteGrande)
                {
                    row = ((carteGrande)((Label)sender).Parent.Parent).drow;
                }
            }

            Accueil.FeuilleRecette f = new Accueil.FeuilleRecette(row);
            f.Location = new Point(0,0);
            this.Clear();
            this.Controls.Add(f);
        }

        public void Clear()
        {
            while(this.Controls.Count>1){
                foreach (Control c in this.Controls)
                {
                    if (!(c is ucBarre))
                    {
                        this.Controls.Remove(c);
                    }
                }
            } 
        }

        public void Click_Recherche_Ingredient(object sender, EventArgs e)
        {
            this.Clear();
            
            ucRechercheIngredient obj = new ucRechercheIngredient(dataset.Tables["Famille"], dataset.Tables["ingrédients"]);
            
            this.Controls.Add(obj);
        }
    }
    
}
