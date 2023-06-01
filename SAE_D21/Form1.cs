using Accueil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SAE_D21
{
    public partial class AppFrigo : Form
    {

        public AppFrigo()
        {
            InitializeComponent();
        }

        ucBarre barre;
        private void AppCuisine_Load(object sender, EventArgs e)
        {
            this.loadDataset();
            
            folderBrowserDialog.SelectedPath = "C:\\Users\\arnaudmichel\\source\\repos\\SAE_D21\\SAE_D21\\pdfRecettes";

            barre = new ucBarre();
            barre.SetClick_Home(this.Click_Home);
            barre.SetClick_Categorie(this.Click_Categorie);
            barre.SetClick_Filtre(this.Click_Recherche_Ingredient);
            barre.Location = new Point(0, 642);
            this.Controls.Add(barre);
            this.loadmenu();

            Button button = new Button();
            button.Text = "Ajouter une recette";
            button.Size = new Size(150, 50);
            button.Location = new Point(0, 0);
            button.Click += testClick;
            this.Controls.Add(button);

            rechercheIng = new ucRechercheIngredient(dataset.Tables["Famille"], dataset.Tables["ingrédients"], Recherche_Ingredient);
            CategoriePage = new ucCategorie(dataset.Tables["Catégories"], Click_Recherche_Categorie);
        }

        ucRechercheIngredient rechercheIng;
        ucCategorie CategoriePage;
        String rechercheSetting = "";


        private void testClick(object sender, EventArgs e)
        {
            BindingS binding = new BindingS(this, dataset, 1);
        }

        private int idAccount = -1;
        Random rnd = new Random();
        

        private void loadmenu()
        {
            Accueil.BarDeRecherche barDeRecherche1 = new Accueil.BarDeRecherche();
            barDeRecherche1.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barDeRecherche1_KeyPress);
            barDeRecherche1.Location = new Point((this.Width - barDeRecherche1.Width) / 2 - 28, 87);


            //Dessin button filtre
            Panel boutton_filtre = new Panel();
            boutton_filtre.Size = new Size(37, 37);
            boutton_filtre.BackColor = Color.Gray;
            boutton_filtre.Location = new Point(barDeRecherche1.Location.X + barDeRecherche1.Width + 15, barDeRecherche1.Location.Y);

            Panel boutton_filtre2 = new Panel();
            boutton_filtre2.Size = new Size(35, 35);
            boutton_filtre2.BackColor = Color.White;
            boutton_filtre2.Location = new Point(1, 1);


            boutton_filtre.Paint += this.panel_Paint;
            boutton_filtre2.Paint += this.panel_Paint;


            PictureBox image_filtre = new PictureBox();
            image_filtre.Image = Image.FromFile("../../assets/Logos/apps.png");
            image_filtre.Size = new Size(25, 25);
            image_filtre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            image_filtre.Location = new Point(5, 5);

            boutton_filtre2.Click += Click_Recherche_Ingredient;
            image_filtre.Click += Click_Recherche_Ingredient;

            boutton_filtre2.Click += barre.Click_filtre;
            image_filtre.Click += barre.Click_filtre;





            this.Controls.Add(boutton_filtre);

            boutton_filtre.Controls.Add(boutton_filtre2);
            boutton_filtre2.Controls.Add(image_filtre);

            PictureBox imageAccount = new PictureBox();
            imageAccount.Image = Image.FromFile("../../assets/account/user.png");
            imageAccount.Size = new Size(40, 40);
            imageAccount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            imageAccount.Location = new Point(Width - 79, 15);
            imageAccount.Tag = "user";
            imageAccount.Click += user_Click;
            this.Controls.Add(imageAccount);

            setImageAccount();


            Label Titre = new Label();
            Titre.Size = new System.Drawing.Size(500, 42);
            Titre.Text = "Qu'est-ce qu'on mange ce soir ?";
            Titre.Font = new System.Drawing.Font("Bahnschrift", 22, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            Titre.TextAlign = ContentAlignment.MiddleCenter;
            Titre.Location = new Point((this.Width) / 2 - (Titre.Size.Width) / 2, 25);

            Label Titre2 = new Label();
            Titre2.Size = new System.Drawing.Size(300, 42);
            Titre2.Text = "Nos recommandations";
            Titre2.Font = new System.Drawing.Font("Bahnschrift", 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            Titre2.Location = new Point((this.Width) / 2 - (Titre2.Width) / 2 - 15, 140);
            Titre2.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(Titre); this.Controls.Add(Titre2);

            Panel panel = new Panel();
            panel.BackColor = Color.FromArgb(255, 0, 0, 0);
            panel.Width = 896;
            panel.Height = 2;
            panel.Location = new Point((this.Width) / 2 - (panel.Width) / 2, 160);
            this.Controls.Add(panel);
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
                    }
                    catch (Exception)
                    {

                    }
                    ucCarte.Location = new Point(25 + j * ucCarte.Width + decale_x, 430 + i * ucCarte.Height + decale_y);
                    ucCarte.setClick(carteGrande_Click);
                    this.Controls.Add(ucCarte);
                }

            }
            Panel panel2 = new Panel();
            panel2.BackColor = Color.FromArgb(255, 128, 128, 128);
            panel2.Width = 1000; 
            panel2.Height = 1;
            panel2.Location = new Point((this.Width) / 2 - (panel2.Width) / 2 - 5, 405);
            this.Controls.Add(panel2);

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
                catch (Exception)
                {

                }
                this.Controls.Add(carteGrande);
            }

        }

        // Connection string to database file
        string chcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=baseFrigov2023.mdb";
        OleDbConnection con = new OleDbConnection();
        DataSet dataset = new DataSet();


        //Variable, tableau
        string[] ingredients = new string[3];
        DataRow[] rowingredient = new DataRow[3];
        List<DataRow> recettes = new List<DataRow>();

        private void user_Click(object sender, EventArgs e)
        {
            Login form1 = new Login(dataset);
            form1.ShowDialog();
            if (form1.DialogResult == DialogResult.OK)
            {
                idAccount = form1.Id;
                setImageAccount();
                
            }
            //this.Close();
        }

        private void setImageAccount()
        {
            if (idAccount > 0)
            {
                foreach (Control ctr in this.Controls.OfType<PictureBox>())
                {
                    if (ctr.Tag.ToString() == "user")
                    {
                        ((PictureBox)ctr).Image = Image.FromFile("../../assets/account/userCo.png");
                    }
                }
                foreach (Control ctr in this.Controls.OfType<Label>())
                {
                    try
                    {
                        if (ctr.Tag != null && ctr.Tag.ToString() == "user")
                        {
                            this.Controls.Remove(ctr);
                        }
                    }catch (Exception)
                    {

                    }
                }
                Label label = new Label();
                label.Tag = "user";
                label.Text = dataset.Tables["User"].Select("codeUser = " + idAccount)[0]["prenom"].ToString();
                label.Font = new Font("Bahnschrift", 12, FontStyle.Bold);
                label.Location = new Point(this.Width - 122, 50);
                label.Size = new Size(128, 32);
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.AutoSize = false;
                this.Controls.Add(label);
            }
            else
            {
                foreach (Control ctr in this.Controls.OfType<PictureBox>())
                {
                    if (ctr.Tag.ToString() == "user")
                    {
                        ((PictureBox)ctr).Image = Image.FromFile("../../assets/account/user.png");
                    }
                }
            }
        }

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
            con.Close();
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

        

        private void rechercher(System.Windows.Forms.TextBox searchbar)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            
            //===== Mode Connecté =====
            // Remov all the uccarte on the screen
            recettes.Clear();
            ingredients = new string[3];
            rowingredient = new DataRow[3];

            String command;
            OleDbCommand cmd;
            OleDbDataAdapter adapter ;


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
                    command = "SELECT * FROM Ingrédients WHERE libIngredient = '" + ingredients[i] + "'";
                    cmd = new OleDbCommand(command, con);
                    DataTable dt = new DataTable();
                    adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        errorProvider.SetError(searchbar.Parent.Parent, "L'ingrédient " + ingredients[i] + " n'  existe pas");
                        searchbar.Parent.Parent.BackColor = Color.IndianRed;
                        searchbar.ForeColor = Color.IndianRed;
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
            if (con.State == ConnectionState.Open)
            {
                con.Close();
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
            }
            else if (e.KeyChar == (char)Keys.Back && searchbar.Text.Trim().Length == 1 && recettes.Count > 0)
            {
                this.Clear();
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
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            // ====== Mode Connecté ======
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            List<DataRow> ingrédientsrecette = new List<DataRow>();
            String command = "";
            recettes.Clear();
            foreach (DataRow ingredient in rowingredient)
            {
                if (ingredient == null)
                {
                    break;
                }
                
                command = "SELECT * FROM recettes WHERE codeRecette IN(SELECT codeRecette FROM IngrédientsRecette WHERE codeIngredient = " + ingredient["codeIngredient"] + ")" + rechercheSetting;
                cmd.CommandText = command;
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!recettes.Contains(dataset.Tables["recettes"].Select("codeRecette = " + reader["codeRecette"])[0]))
                    recettes.Add(dataset.Tables["recettes"].Select("codeRecette = " + reader["codeRecette"])[0]);
                }
                reader.Close();
            }
            Panel pnl = new Panel();
            pnl.Size = new Size(1080, 440);
            pnl.Location = new Point(0, 200);
            pnl.AutoScroll = true;
            pnl.BackColor = Color.Transparent;
            this.Clear();
            this.Controls.Add(pnl);
            con.Close();
            Label Titre = new Label();
            Titre.Size = new System.Drawing.Size(500, 42);
            Titre.Text = "Qu'est-ce qu'on mange ce soir ?";
            Titre.Font = new System.Drawing.Font("Bahnschrift", 22, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            Titre.TextAlign = ContentAlignment.MiddleCenter;
            Titre.Location = new Point((this.Width) / 2 - (Titre.Size.Width) / 2, 25);

            Label Titre2 = new Label();
            Titre2.Size = new System.Drawing.Size(300, 42);
            Titre2.Text = "Résultats";
            Titre2.Font = new System.Drawing.Font("Bahnschrift", 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            Titre2.Location = new Point((this.Width) / 2 - (Titre2.Width) / 2 - 15, 140);
            Titre2.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(Titre); this.Controls.Add(Titre2);

            Panel panel = new Panel();
            panel.BackColor = Color.FromArgb(255, 0, 0, 0);
            panel.Width = 896;
            panel.Height = 2;
            panel.Location = new Point((this.Width) / 2 - (panel.Width) / 2, 160);
            this.Controls.Add(panel);

            for (int i = 0; i < recettes.Count; i++)
            {
                DataRow row = recettes[i];
                ucCarteEtoile carte = this.createCarteStars(row, 50 + (i% 3) * 330, 0 + (i/3) * 100);
                pnl.Controls.Add(carte);
            }
            select = -1;
            

            Accueil.BarDeRecherche barDeRecherche1 = new Accueil.BarDeRecherche();
            barDeRecherche1.textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.barDeRecherche1_KeyPress);
            barDeRecherche1.Location = new Point((this.Width - barDeRecherche1.Width) / 2 - 28, 87);
            this.Controls.Add(barDeRecherche1);


            //Dessin button filtre
            Panel boutton_filtre = new Panel();
            boutton_filtre.Size = new Size(37, 37);
            boutton_filtre.BackColor = Color.Gray;
            boutton_filtre.Location = new Point(barDeRecherche1.Location.X + barDeRecherche1.Width + 15, barDeRecherche1.Location.Y);

            Panel boutton_filtre2 = new Panel();
            boutton_filtre2.Size = new Size(35, 35);
            boutton_filtre2.BackColor = Color.White;
            boutton_filtre2.Location = new Point(1, 1);


            boutton_filtre.Paint += this.panel_Paint;
            boutton_filtre2.Paint += this.panel_Paint;


            PictureBox image_filtre = new PictureBox();
            image_filtre.Image = Image.FromFile("../../assets/Logos/settings.png");
            image_filtre.Size = new Size(25, 25);
            image_filtre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            image_filtre.Location = new Point(5, 5);

            boutton_filtre2.Click += Click_Recherche_Ingredient;
            image_filtre.Click += Click_Recherche_Ingredient;

            this.Controls.Add(boutton_filtre);

            boutton_filtre.Controls.Add(boutton_filtre2);
            boutton_filtre2.Controls.Add(image_filtre);
        }

        private void carteGrande_Click(object sender, EventArgs e)
        {
            DataRow row = dataset.Tables["Recettes"].Rows[0];

            if (sender is Panel)
            {
                if (((Panel)sender).Parent is ucCarte)
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
            else if (sender is UserControl)
            {
                row = ((carteGrande)sender).drow;
            }
            else if (sender is PictureBox)
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
            else if (sender is Label)
            {

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
            select = -1;
        }

        public void Clear()
        {
            while (this.Controls.Count > 1)
            {
                foreach (Control c in this.Controls)
                {
                   
                    if (!(c is ucBarre))
                    {
                        this.Controls.Remove(c);
                    }

                }
            }
        }
        public void Clear_Menu()
        {
            while (this.Controls.Count > 2)
            {
                foreach (Control c in this.Controls)
                {
                    
                    if (!(c is ucBarre || c is BarDeRecherche))
                    {
                        this.Controls.Remove(c);
                    }

                }
            }
        }



        private int select = 1;
        public void Click_Recherche_Ingredient(object sender, EventArgs e)
        {
            if (select != 5)
            {
                this.Clear();

                this.Controls.Add(rechercheIng);
                select = 5;
            }
            

        }

        public void Click_Recherche_Categorie(object sender, EventArgs e)
        {
            Accueil.ucCategorie.Return @return = ((ucCategorie)((Label)sender).Parent.Parent.Parent).Requete;
            StructToStr(@return);
            MessageBox.Show("Requete: " + rechercheSetting);
        }

        public void Recherche_Ingredient(object sender, EventArgs e)
        {
            ucRechercheIngredient ri;
            if(sender is Panel)
            {
                ri = ((Panel)sender).Parent.Parent as ucRechercheIngredient;
            }
            else
            {
                ri = ((Label)sender).Parent.Parent.Parent as ucRechercheIngredient;
            }
            rowingredient = ri.Ingredient;
            this.listeIngredientInRecette();
        }

        public void Click_Home(object sender, EventArgs e)
        {
            if (select != 1)
            {
                this.Clear();

                this.loadmenu();
                select = 1;
            }


        }
        public void Click_Categorie(object sender, EventArgs e)
        {
            if (select != 2)
            {
                this.Clear();
                
                this.Controls.Add(CategoriePage);
                select = 2;
            }

        }
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            int radius = 10; // Rayon des bords ronds
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddLine(radius, 0, ((Panel)sender).Width - radius, 0);
            path.AddArc(((Panel)sender).Width - radius, 0, radius, radius, 270, 90);
            path.AddLine(((Panel)sender).Width, radius, ((Panel)sender).Width, ((Panel)sender).Height - radius);
            path.AddArc(((Panel)sender).Width - radius, ((Panel)sender).Height - radius, radius, radius, 0, 90);
            path.AddLine(((Panel)sender).Width - radius, ((Panel)sender).Height, radius, ((Panel)sender).Height);
            path.AddArc(0, ((Panel)sender).Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            ((Panel)sender).Region = new Region(path);
        }

        public void StructToStr(Accueil.ucCategorie.Return @return)
        {
            rechercheSetting = "";
            bool first = true;
            if (@return.codeCatego.Count > 0)
            {
                foreach (int i in @return.codeCatego)
                {
                    if (first)
                    {
                        rechercheSetting += " AND (codeRecette IN (SELECT codeRecette FROM CatégoriesRecette WHERE codeCategorie = " + i + ") ";
                        first = false;
                    }
                    else
                    {
                        rechercheSetting += " OR codeRecette IN (SELECT codeRecette FROM CatégoriesRecette WHERE codeCategorie = " + i + ") ";
                    }
                }
                rechercheSetting += ")";
            }
            rechercheSetting += " AND categPrix <= " + @return.prix;
            if (@return.temps != 0)
            {
                rechercheSetting += " AND tempsCuisson <= " + @return.temps;
            }
        }
    }

}
