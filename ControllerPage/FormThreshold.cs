using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControllerPage.Library;
using ControllerPage.Helper;

namespace ControllerPage
{
    public partial class FormThreshold : Form
    {
        public FormThreshold()
        {
            InitializeComponent();
            Data_initialization_Thereshold();
        }


        private void Data_initialization_Thereshold()
        {
            List<SQL_Data_Config> current_config = Sensor_input_Helper.MySql_Get_DataConfig(Sensor_input_Helper.GetLocalIPAddress());

            var TheresholdMax_var = current_config.Where(config => config.Config_Param == "Thereshold_Max");
            double TheresholdMax_value = (TheresholdMax_var.Select(p => p.Config_Value).ToArray()).First();

            var TheresholdMin_var = current_config.Where(config => config.Config_Param == "Thereshold_Min");
            double TheresholdMin_value = (TheresholdMin_var.Select(p => p.Config_Value).ToArray()).First();

            var TheresholdEnable_var = current_config.Where(config => config.Config_Param == "Thereshold_Enable");
            double TheresholdEnable_value = (TheresholdEnable_var.Select(p => p.Config_Value).ToArray()).First();


            button_thereshold_maxvalue.Text = TheresholdMax_value.ToString() + "%";
            button_thereshold_minvalue.Text = TheresholdMin_value.ToString() + "%";

            if (TheresholdEnable_value == 1)
            {
                radioButton_enable.Checked = true;
                radioButton_disable.Checked = false;

            }
            else
            {
                radioButton_enable.Checked = false;
                radioButton_disable.Checked = true;

            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button_TheresholdApply_Click(object sender, EventArgs e)
        {
            string minvalue_str = button_thereshold_minvalue.Text.Remove(button_thereshold_minvalue.Text.Length - 1, 1);
            string maxvalue_str = button_thereshold_maxvalue.Text.Remove(button_thereshold_maxvalue.Text.Length - 1, 1);
            Global.GlobalVar11 = button_thereshold_minvalue.Text;
            Global.GlobalVar12 = button_thereshold_maxvalue.Text;
            if (double.Parse(maxvalue_str) > 40 || double.Parse(minvalue_str) < 11)
            {
                MessageBox.Show("Thereshold Range is 11% to 40%");
            }
            else
            {
                double value_theresholdenable = 0;
                if (radioButton_enable.Checked == true)
                {
                    value_theresholdenable = 1;
                }
                else
                {
                    value_theresholdenable = 0;
                }
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "Thereshold_Enable"
                    , value_theresholdenable.ToString());
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "Thereshold_Max"
                    , maxvalue_str);

                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "Thereshold_Min"
                    , minvalue_str);

                this.Close();

            }


        }

        private void button_maxthereshold_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var form = new FormNumpad_max())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    button_thereshold_maxvalue.Text = FormNumpad_max.numpad_max  + "%";

                }
            }
            this.Show();
        }

        private void button_minthereshold_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var form = new FormNumpad_min())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    button_thereshold_minvalue.Text = FormNumpad_min.numpad_min + "%";
                }
            }
            this.Show();
        }

        private void radioButton_enable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Btn_close_thereshold_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormThreshold_Load(object sender, EventArgs e)
        {
            if (Global.GlobalVar == "Bahasa")
            {
                label1.Text = "Setting Ambang Batas";
                radioButton_enable.Text = "Aktif";
                radioButton_disable.Text = "NonAktif";
                label2.Text = "Nilai Minimum";
                label3.Text = "Nilai Maksimum";
                button1.Text = "Simpan";
                button_thereshold_apply.Text = "Tutup";
            }
            if (Global.GlobalVar == "English")
            {
                label1.Text = "Threshold Setting";
                radioButton_enable.Text = "Enable";
                radioButton_disable.Text = "Disable";
                label2.Text = "Minimum Value";
                label3.Text = "Maximum Value";
                button1.Text = "Save";
                button_thereshold_apply.Text = "Close";
            }
        }
    }
}
