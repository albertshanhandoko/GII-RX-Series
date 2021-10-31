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
    public partial class FormLanguage : Form
    {
        public FormLanguage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Global.GlobalVar = "Bahasa";
            Global.GlobalVar1 = "Pengukuran Kadar Air Aktual";
            Global.GlobalVar2 = "Interval Ke";
            Global.GlobalVar3 = "Penghitung Biji";
            Global.GlobalVar4 = "Rataan Kadar Air";
            Global.GlobalVar5 = "Produk";
            Global.GlobalVar6 = "Jumlah Interval";
            Global.GlobalVar7 = "PCS tiap Interval";
            Global.GlobalVar8 = "Waktu Tunggu";
            Global.GlobalVar9 = "Opsi";
            Global.GlobalVar10 = "Nomor Sensor";
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Global.GlobalVar = "English";
            Global.GlobalVar1 = "Current Interval Moisture Measurement";
            Global.GlobalVar2 = "Interval Number";
            Global.GlobalVar3 = "Kernel Counter";
            Global.GlobalVar4 = "Average Moisture";
            Global.GlobalVar5 = "Product";
            Global.GlobalVar6 = "No. of Intervals";
            Global.GlobalVar7 = "PCS per Interval";
            Global.GlobalVar8 = "Int. Waiting Time";
            Global.GlobalVar9 = "Options";
            Global.GlobalVar10 = "Sensor Number";
            this.Close();
        }
    }
}
