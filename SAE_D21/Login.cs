using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAE_D21
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        DataSet dataSet;
        private int idAccount;

        public Login(DataSet dataSet)
        {
            InitializeComponent();
            this.dataSet = dataSet;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataSet.Tables["User"].Select("pseudo = '" + textBox1.Text + "' AND password = '" + textBox2.Text + "'").Length > 0)
            {
                this.Hide();
                this.Id = Convert.ToInt32(dataSet.Tables["User"].Select("pseudo = '" + textBox1.Text + "' AND password = '" + textBox2.Text + "'")[0]["codeUser"]);
                MessageBox.Show("Bienvenue " + textBox1.Text);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Login ou mot de passe incorrect");
            }
        }

        public int Id
        {
            get { return idAccount; }
            set { idAccount = value; }
        }
    }
}
