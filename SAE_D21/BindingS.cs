using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accueil;
using SAE_D21;

namespace SAE_D21
{
    internal class BindingS
    {
        BindingSource bs = new BindingSource();
        public BindingS(Form form, DataSet ds, int i)
        {
            clearctrl(form);
            Label lblNomRecette = new Label();
            lblNomRecette.Text = ds.Tables["Recettes"].Select("codeRecette = '" + i + "'")[0]["description"].ToString();
            lblNomRecette.Font = new System.Drawing.Font("Bahnschrift", 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblNomRecette.Location = new System.Drawing.Point(form.Width / 2 - lblNomRecette.Text.Length * 5, 50);
            lblNomRecette.AutoSize = true;
            form.Controls.Add(lblNomRecette);

            DataTable dt = new DataTable();
            dt.TableName = "EtapesRecette";
            //Definir les colonnes de la table comme celle de la table Etape du dataset
            dt = ds.Tables["Etapesrecette"].Select("codeRecette = '" + i + "'").CopyToDataTable();

            bs.DataSource = dt;

            Label lblIdEtape = new Label();
            lblIdEtape.DataBindings.Add("Text", bs, "numEtape");
            lblIdEtape.Location = new System.Drawing.Point(50, 100);
            lblIdEtape.AutoSize = true;
            form.Controls.Add(lblIdEtape);

            Label lblEtape = new Label();
            lblEtape.DataBindings.Add("Text", bs, "texteEtape");
            lblEtape.Location = new System.Drawing.Point(50, 150);
            lblEtape.AutoSize = true;
            form.Controls.Add(lblEtape);

            Button btnFirst = new Button();
            btnFirst.Text = "Début";
            btnFirst.Location = new System.Drawing.Point(50, 200);
            btnFirst.Click += new EventHandler(First);
            form.Controls.Add(btnFirst);

            Button btnPrevious = new Button();
            btnPrevious.Text = "Précedent";
            btnPrevious.Location = new System.Drawing.Point(150, 200);
            btnPrevious.Click += new EventHandler(Previous);
            form.Controls.Add(btnPrevious);

            Button btnNext = new Button();
            btnNext.Text = "Suivant";
            btnNext.Location = new System.Drawing.Point(250, 200);
            btnNext.Click += new EventHandler(Next);
            form.Controls.Add(btnNext);

            Button btnLast = new Button();
            btnLast.Text = "Fin";
            btnLast.Location = new System.Drawing.Point(350, 200);
            btnLast.Click += new EventHandler(Last);
            form.Controls.Add(btnLast);

        }

        private void Next(object sender, EventArgs e)
        {
            bs.MoveNext();
        }

        private void Previous(object sender, EventArgs e)
        {
            bs.MovePrevious();
        }

        private void First(object sender, EventArgs e)
        {
            bs.MoveFirst();
        }

        private void Last(object sender, EventArgs e)
        {
            bs.MoveLast();
        }

        private void clearctrl(Form form)
        {
            bool control = true;
            while (control)
            {
                control = false;
                foreach (Control c in form.Controls)
                {
                    form.Controls.Remove(c);
                    control = true;
                    
                }
            }
        }

    }

}
