using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControllerPage
{
    public partial class FormProductselection_V010 : Form
    {
        public FormProductselection_V010()
        {
            InitializeComponent();
        }
        public string Productselection { get; set; }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Productselection = button2.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Productselection = button3.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Productselection = button4.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Productselection = button5.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Productselection = button6.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Productselection = button7.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Productselection = button1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormProductselection_Load(object sender, EventArgs e)
        {
            if (Global.GlobalVar == "Bahasa")
            {
                label1.Text = "Pilih Produk";
                button2.Text = "Padi";
                button3.Text = "Jagung Tinggi";
                button4.Text = "Jagung Medium";
                button5.Text = "Barley";
                button6.Text = "Kedelai";
                button7.Text = "Jagung Rendah";
                button1.Text = "Beras Poles";
            }
            if (Global.GlobalVar == "English")
            {
                label1.Text = "Product Selection";
                button2.Text = "Paddy";
                button3.Text = "Corn High";
                button4.Text = "Corn Medium";
                button5.Text = "Barley";
                button6.Text = "Soy";
                button7.Text = "Corn Low";
                button1.Text = "Polished Rice";
            }
        }
    }
}
