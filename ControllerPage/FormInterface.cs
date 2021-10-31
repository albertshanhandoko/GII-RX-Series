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
    public partial class FormInterface : Form
    {
        public FormInterface()
        {
            InitializeComponent();
        }
        public string Interfaceselection { get; set; }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Interfaceselection = "RS-232";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Interfaceselection = "RS-485";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
