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
    public partial class FormMode : Form
    {
        public FormMode()
        {
            InitializeComponent();
        }
        public string ModeSelection { get; set; }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ModeSelection = "Interval";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ModeSelection = "Fixed Time";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ModeSelection = "Fixed Pieces";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormMode_Load(object sender, EventArgs e)
        {
            if (Global.GlobalVar == "Bahasa")
            {
                label1.Text = "Mode Pengukuran";
                button1.Text = "PCS Tetap";
                button3.Text = "Waktu Tetap";

            }
            if (Global.GlobalVar == "English")
            {
                label1.Text = "Measure Mode";
                button1.Text = "Fixed PCS";
                button3.Text = "Fixed Time";
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.ModeSelection = "Fixed Pieces";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
