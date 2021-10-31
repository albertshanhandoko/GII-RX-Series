using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControllerPage.Helper;
namespace ControllerPage
{


    public partial class FormProductselection : Form
    {
        public string Productselection { get; set; }

        public FormProductselection()
        {
            InitializeComponent();

            // jenis product
            // 1 = Corn
            // 2 = Corn High
            // 3 = short Paddy

            int product_type = SensorHelper_2.Get_ProductType();

            if (Global.GlobalVar == "Bahasa")
            {
                if (product_type == 1)
                {
                    button1.Text = "Padi";
                    button2.Text = "Beras Merah";
                    button3.Text = "Gandum";
                    button4.Text = "Barley";
                    button5.Text = "Kedelai";
                    button6.Text = "Jagung";
                    button7.Text = "Beras Poles";
                    button8.Enabled = false;
                    button9.Enabled = false;
                    //button8.Text = "Long Milled Rice";
                    //button9.Text = "Long Brown Rice";

                }
                else if (product_type == 2)
                {
                    button1.Text = "Padi";
                    button2.Text = "Jagung Tinggi";
                    button3.Text = "Jagung Medium";
                    button4.Text = "Barley";
                    button5.Text = "Kedelai";
                    button6.Text = "Jagung Low";
                    button7.Text = "Beras Poles";
                    button8.Enabled = false;
                    button9.Enabled = false;
                    button8.Text = "";
                    button9.Text = "";

                }
                else //3
                {
                    button1.Text = "Padi Pendek";
                    button2.Text = "Padi Panjang";
                    button3.Text = "Padi Jasmine";
                    button4.Text = "Padi Long Sticky";
                    button5.Text = "Beras Long Parboiled";
                    button6.Text = "Peak AD Count";
                    button7.Text = "Gandum";
                    button8.Text = "Beras Long Milled";
                    button9.Text = "Beras Long Brown";

                }

            }
            else
            { 
                if (product_type == 1)
            {
                button1.Text = "Paddy";
                button2.Text = "Brown Rice";
                button3.Text = "Wheat";
                button4.Text = "Barley";
                button5.Text = "Soy";
                button6.Text = "Corn";
                button7.Text = "Polished Rice";
                button8.Visible = false;
                button9.Visible = false;
                button8.Text = "";
                button9.Text = "";

                }
            else if(product_type == 2)
            {
                button1.Text = "Paddy";
                button2.Text = "Corn High";
                button3.Text = "Corn Medium";
                button4.Text = "Barley";
                button5.Text = "Soy";
                button6.Text = "Corn Low";
                button7.Text = "Polished Rice";
                button8.Visible = false;
                button9.Visible = false;
                button8.Text = "";
                button9.Text = "";

            }
            else //3
            {
                button1.Text = "Short Paddy";
                button2.Text = "Long Paddy";
                button3.Text = "Jasmine Paddy";
                button4.Text = "Long Sticky Paddy";
                button5.Text = "Long Parboiled Rice";
                button6.Text = "Peak AD Count";
                button7.Text = "Wheat";
                button8.Text = "Long Milled Rice";
                button9.Text = "Long Brown Rice";

            }
            
            }
        }
        
        

        private void button2_Click(object sender, EventArgs e)
        {
            //this.Productselection = "Paddy";
            this.Productselection = button2.Text.Replace(" ", "_");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.Productselection = "Corn_High";
            this.Productselection = button3.Text.Replace(" ", "_");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //this.Productselection = "Corn_Medium";
            this.Productselection = button4.Text.Replace(" ", "_");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //this.Productselection = "Barley";
            this.Productselection = button5.Text.Replace(" ", "_");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //this.Productselection = "Soy";
            this.Productselection = button6.Text.Replace(" ", "_");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //this.Productselection = "Corn_Low";
            this.Productselection = button7.Text.Replace(" ", "_");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Productselection = "Polished_Rice";
            this.Productselection = button1.Text.Replace(" ", "_");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Productselection = button8.Text.Replace(" ", "_");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Productselection = button9.Text.Replace(" ", "_");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }




}
