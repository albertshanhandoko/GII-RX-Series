using ControllerPage.Helper;
using ControllerPage.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ControllerPage
{


    public partial class FormBIAS_New : Form
    {
        double Paddy_Value;
        double brownrice_value;
        double wheat_value;
        double barley_value;
        double soy_value;
        double corn_value;
        double polishedrice_value;

        string var_label1;
        string var_label2;
        string var_label3;
        string var_label4;
        string var_label5;
        string var_label6;
        string var_label7;
        string var_label8;
        string var_label9;
        int product_type;

        bool _runPaddy;
        public FormBIAS_New()
        {
            InitializeComponent();
            DataInitialization_Bias();
        }


        private void DataInitialization_Bias()
        {
            ///
            product_type = SensorHelper_2.Get_ProductType();
            if (product_type == 1)
            {
                var_label1  = "Paddy";
                var_label2 = "Brown Rice";
                var_label3 = "Wheat";
                var_label4 = "Barley";
                var_label5 = "Soy";
                var_label6 = "Corn";
                var_label7 = "Polished Rice";


                label_1.Text = var_label1;
                label_2.Text = var_label2;
                label_3.Text = var_label3;
                label_4.Text = var_label4;
                label_5.Text = var_label5;
                label_6.Text = var_label6;
                label_7.Text = var_label7;

                if (Global.GlobalVar == "Bahasa")
                {
                    label_title.Text = "Setting BIAS";
                    label_1.Text = "Padi";
                    label_2.Text = "Beras Merah";
                    label_3.Text = "Gandum";
                    label_4.Text = "Barley";
                    label_5.Text = "Kedelai";
                    label_6.Text = "Jagung";
                    label_7.Text = "Beras Poles";
                    button_bias_apply.Text = "Simpan";
                    BIAS_New_Close.Text = "Tutup";
                }


                label_8.Visible = false;
                button8_minus.Visible = false;
                button8_plus.Visible = false;
                textBox_Bias_longmilledrice.Visible = false;

                label_9.Visible = false;
                button9_minus.Visible = false;
                button9_plus.Visible = false;
                textBox_Bias_shortmilledrice.Visible = false;

                //button8.Text = "Long Milled Rice";
                //button9.Text = "Long Brown Rice";

            }
            else if (product_type == 2)
            {

                var_label1 = "Paddy";
                var_label2 = "Corn High";
                var_label3 = "Corn Medium";
                var_label4 = "Barley";
                var_label5 = "Soy";
                var_label6 = "Corn Low";
                var_label7 = "Polished Rice";


                label_1.Text = var_label1;
                label_2.Text = var_label2;
                label_3.Text = var_label3;
                label_4.Text = var_label4;
                label_5.Text = var_label5;
                label_6.Text = var_label6;
                label_7.Text = var_label7;

                if (Global.GlobalVar == "Bahasa")
                {
                    label_title.Text = "Setting BIAS";
                    label_1.Text = "Padi";
                    label_2.Text = "Jagung Tinggi";
                    label_3.Text = "Jagung Medium";
                    label_4.Text = "Barley";
                    label_5.Text = "Kedelai";
                    label_6.Text = "Jagung Rendah";
                    label_7.Text = "Beras Poles";
                    button_bias_apply.Text = "Simpan";
                    BIAS_New_Close.Text = "Tutup";
                }


                label_8.Visible = false;
                button8_minus.Visible = false;
                button8_plus.Visible = false;
                textBox_Bias_longmilledrice.Visible = false;

                label_9.Visible = false;
                button9_minus.Visible = false;
                button9_plus.Visible = false;
                textBox_Bias_shortmilledrice.Visible = false;

            }
            else //3
            {

                var_label1 = "Short Paddy";
                var_label2 = "Long Paddy";
                var_label3 = "Jasmine Paddy";
                var_label4 = "Long Sticky Paddy";
                var_label5 = "Long Parboiled Rice";
                var_label6 = "Peak AD Count";
                var_label7 = "Wheat";
                var_label8 = "Long Milled Rice";
                var_label9 = "Long Brown Rice";


                label_1.Text = var_label1;
                label_2.Text = var_label2;
                label_3.Text = var_label3;
                label_4.Text = var_label4;
                label_5.Text = var_label5;
                label_6.Text = var_label6;
                label_7.Text = var_label7;
                label_8.Text = var_label8;
                label_9.Text = var_label9;

                if (Global.GlobalVar == "Bahasa")
                {
                    label_title.Text = "Setting BIAS";
                    label_1.Text = "Padi Pendek";
                    label_2.Text = "Padi Panjang";
                    label_3.Text = "Padi Jasmine";
                    label_4.Text = "Padi Long Sticky";
                    label_5.Text = "Padi Long Parboiled";
                    label_6.Text = "Peak AD Count";
                    label_7.Text = "Gandum";
                    label_8.Text = "Padi Panjang Milled";
                    label_9.Text = "Padi Panjang Merah";
                    button_bias_apply.Text = "Simpan";
                    BIAS_New_Close.Text = "Tutup";
                    
                }

            }

            ///


            List<SQL_Data_Config> current_config = Sensor_input_Helper.MySql_Get_DataConfig(Sensor_input_Helper.GetLocalIPAddress());

            var var1 = current_config.Where(config => config.Config_Param == var_label1.ToLower().Replace(" ","_") );
            double var_value1 = (var1.Select(p => p.Config_Value).ToArray()).First();

            var var2 = current_config.Where(config => config.Config_Param == var_label2.ToLower().Replace(" ", "_"));
            double var_value2 = (var2.Select(p => p.Config_Value).ToArray()).First();

            var var3 = current_config.Where(config => config.Config_Param == var_label3.ToLower().Replace(" ", "_"));
            double var_value3 = (var3.Select(p => p.Config_Value).ToArray()).First();

            var var4 = current_config.Where(config => config.Config_Param == var_label4.ToLower().Replace(" ", "_"));
            double var_value4 = (var4.Select(p => p.Config_Value).ToArray()).First();

            var var5 = current_config.Where(config => config.Config_Param == var_label5.ToLower().Replace(" ", "_"));
            double var_value5 = (var5.Select(p => p.Config_Value).ToArray()).First();

            var var6 = current_config.Where(config => config.Config_Param == var_label6.ToLower().Replace(" ", "_"));
            double var_value6 = (var6.Select(p => p.Config_Value).ToArray()).First();

            var var7 = current_config.Where(config => config.Config_Param == var_label7.ToLower().Replace(" ", "_"));
            double var_value7 = (var7.Select(p => p.Config_Value).ToArray()).First();
            
            /*
            var LongPaddy_var = current_config.Where(config => config.Config_Param == "long_paddy");
            double LongPaddy_value = (LongPaddy_var.Select(p => p.Config_Value).ToArray()).First();

            var Wheat_var = current_config.Where(config => config.Config_Param == "wheat");
            double Wheat_value = (Wheat_var.Select(p => p.Config_Value).ToArray()).First();

            var LongStickyPaddy_var = current_config.Where(config => config.Config_Param == "long_sticky_paddy");
            double LongStickyPaddy_value = (LongStickyPaddy_var.Select(p => p.Config_Value).ToArray()).First();

            var LongParboiledRice_var = current_config.Where(config => config.Config_Param == "long_parboiled_rice");
            double LongParboiledRice_value = (LongParboiledRice_var.Select(p => p.Config_Value).ToArray()).First();

            var PeakAdCount_var = current_config.Where(config => config.Config_Param == "peakadcount");
            double PeakAdCount_value = (PeakAdCount_var.Select(p => p.Config_Value).ToArray()).First();

            var JasminePaddy_var = current_config.Where(config => config.Config_Param == "jasmine_paddy");
            double JasminePaddy_value = (JasminePaddy_var.Select(p => p.Config_Value).ToArray()).First();

            var LongMilledRice_var = current_config.Where(config => config.Config_Param == "long_milled_rice");
            double LongMilledRice_value = (LongMilledRice_var.Select(p => p.Config_Value).ToArray()).First();

            var ShortMilledRice_var = current_config.Where(config => config.Config_Param == "short_milled_rice");
            double ShortMilledRice_value = (ShortMilledRice_var.Select(p => p.Config_Value).ToArray()).First();
            */
            textBox_Bias_ShortPaddy.Text = var_value1.ToString();
            textBox_bias_LongPaddy.Text = var_value2.ToString();
            textBox_bias_wheat.Text = var_value3.ToString();
            textBox_bias_LongStickyRice.Text = var_value4.ToString();
            textBox_bias_LongParboiledRice.Text = var_value5.ToString();
            textBox_bias_PeakADCount.Text = var_value6.ToString();
            textBox_bias_JasminePaddy.Text = var_value7.ToString();


            if (product_type == 3)
            {
                var var8 = current_config.Where(config => config.Config_Param == var_label8.ToLower().Replace(" ", "_"));
                double var_value8 = (var8.Select(p => p.Config_Value).ToArray()).First();

                var var9 = current_config.Where(config => config.Config_Param == var_label9.ToLower().Replace(" ", "_"));
                double var_value9 = (var9.Select(p => p.Config_Value).ToArray()).First();
                textBox_Bias_longmilledrice.Text = var_value8.ToString();
                textBox_Bias_shortmilledrice.Text = var_value9.ToString();

            }

            //button_plus_bias_paddy.Hol

        }



        private void button_Bias_Apply_click(object sender, EventArgs e)
        {

        }




        private void FormBIAS_Load(object sender, EventArgs e)
        {

        }

        #region Paddy
        private bool _runpaddy = false;

        private void button_plus_bias_paddy_MouseDown(object sender, MouseEventArgs e)
        {
            _runpaddy = true;
            Thread readThread = new Thread(Paddy_Plus_Async);
            readThread.Start();
        }
        private void button_plus_bias_paddy_MouseUp(object sender, MouseEventArgs e)
        {
            _runpaddy = false;
        }
        public void Paddy_Plus_Async()
        {
            //DateTime Start = DateTime.Now();
            int counter_paddy = 0;
            while (_runpaddy)
            {
                counter_paddy++;
                if (counter_paddy <= 6)
                {
                    Thread.Sleep(300);
                    textBox_Bias_ShortPaddy.Invoke((Action)delegate
                    {
                        textBox_Bias_ShortPaddy.Text = (double.Parse(textBox_Bias_ShortPaddy.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_paddy > 5 & counter_paddy <= 10)
                {
                    Thread.Sleep(200);
                    textBox_Bias_ShortPaddy.Invoke((Action)delegate
                    {
                        textBox_Bias_ShortPaddy.Text = (double.Parse(textBox_Bias_ShortPaddy.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_paddy > 10)
                {
                    Thread.Sleep(50);
                    textBox_Bias_ShortPaddy.Invoke((Action)delegate
                    {
                        textBox_Bias_ShortPaddy.Text = (double.Parse(textBox_Bias_ShortPaddy.Text) + 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }
        private void button_minus_bias_paddy_MouseDown(object sender, MouseEventArgs e)
        {
            _runpaddy = true;
            Thread readThread = new Thread(Paddy_Minus_Async);
            readThread.Start();
        }
        private void button_minus_bias_paddy_MouseUp(object sender, MouseEventArgs e)
        {
            _runpaddy = false;
        }
        public void Paddy_Minus_Async()
        {
            int counter_paddy = 0;
            while (_runpaddy)
            {
                counter_paddy++;
                if (counter_paddy <= 6)
                {
                    Thread.Sleep(300);
                    textBox_Bias_ShortPaddy.Invoke((Action)delegate
                    {
                        textBox_Bias_ShortPaddy.Text = (double.Parse(textBox_Bias_ShortPaddy.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_paddy > 5 & counter_paddy <= 10)
                {
                    Thread.Sleep(200);
                    textBox_Bias_ShortPaddy.Invoke((Action)delegate
                    {
                        textBox_Bias_ShortPaddy.Text = (double.Parse(textBox_Bias_ShortPaddy.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_paddy > 10)
                {
                    Thread.Sleep(50);
                    textBox_Bias_ShortPaddy.Invoke((Action)delegate
                    {
                        textBox_Bias_ShortPaddy.Text = (double.Parse(textBox_Bias_ShortPaddy.Text) - 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }


        #endregion

        #region BrownRice
        private bool _runBrown = false;
        private void btn_plus_brown_MouseDown(object sender, MouseEventArgs e)
        {
            _runBrown = true;
            Thread readThread = new Thread(Brown_Plus_Async);
            readThread.Start();
        }

        private void btn_plus_brown_MouseUp(object sender, MouseEventArgs e)
        {
            _runBrown = false;
        }
        public void Brown_Plus_Async()
        {
            int counter_Brown = 0;
            while (_runBrown)
            {
                counter_Brown++;
                if (counter_Brown <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_LongPaddy.Invoke((Action)delegate
                    {
                        textBox_bias_LongPaddy.Text = (double.Parse(textBox_bias_LongPaddy.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_Brown > 5 & counter_Brown <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_LongPaddy.Invoke((Action)delegate
                    {
                        textBox_bias_LongPaddy.Text = (double.Parse(textBox_bias_LongPaddy.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_Brown > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_LongPaddy.Invoke((Action)delegate
                    {
                        textBox_bias_LongPaddy.Text = (double.Parse(textBox_bias_LongPaddy.Text) + 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }


        private void btn_minus_brown_MouseDown(object sender, MouseEventArgs e)
        {
            _runBrown = true;
            Thread readThread = new Thread(Brown_Minus_Async);
            readThread.Start();
        }

        private void btn_minus_brown_MouseUp(object sender, MouseEventArgs e)
        {
            _runBrown = false;
        }
        public void Brown_Minus_Async()
        {
            int counter_Brown = 0;
            while (_runBrown)
            {
                counter_Brown++;
                if (counter_Brown <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_LongPaddy.Invoke((Action)delegate
                    {
                        textBox_bias_LongPaddy.Text = (double.Parse(textBox_bias_LongPaddy.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_Brown > 5 & counter_Brown <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_LongPaddy.Invoke((Action)delegate
                    {
                        textBox_bias_LongPaddy.Text = (double.Parse(textBox_bias_LongPaddy.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_Brown > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_LongPaddy.Invoke((Action)delegate
                    {
                        textBox_bias_LongPaddy.Text = (double.Parse(textBox_bias_LongPaddy.Text) - 0.1).ToString("0.0");
                    });
                }
            }
        }


        #endregion

        #region Wheat
        private bool _runwheat = false;
        private void btn_plus_wheat_MouseDown(object sender, MouseEventArgs e)
        {
            _runwheat = true;
            Thread readThread = new Thread(wheat_Plus_Async);
            readThread.Start();
        }

        private void btn_plus_wheat_MouseUp(object sender, MouseEventArgs e)
        {
            _runwheat = false;
        }
        public void wheat_Plus_Async()
        {
            int counter_wheat = 0;
            while (_runwheat)
            {
                counter_wheat++;
                if (counter_wheat <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_wheat.Invoke((Action)delegate
                    {
                        textBox_bias_wheat.Text = (double.Parse(textBox_bias_wheat.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_wheat > 5 & counter_wheat <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_wheat.Invoke((Action)delegate
                    {
                        textBox_bias_wheat.Text = (double.Parse(textBox_bias_wheat.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_wheat > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_wheat.Invoke((Action)delegate
                    {
                        textBox_bias_wheat.Text = (double.Parse(textBox_bias_wheat.Text) + 0.1).ToString("0.0");
                    });
                }
            }
        }

        private void btn_minus_wheat_MouseDown(object sender, MouseEventArgs e)
        {
            _runwheat = true;
            Thread readThread = new Thread(wheat_Minus_Async);
            readThread.Start();
        }

        private void btn_minus_wheat_MouseUp(object sender, MouseEventArgs e)
        {
            _runwheat = false;
        }
        public void wheat_Minus_Async()
        {
            int counter_wheat = 0;
            while (_runwheat)
            {
                counter_wheat++;
                if (counter_wheat <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_wheat.Invoke((Action)delegate
                    {
                        textBox_bias_wheat.Text = (double.Parse(textBox_bias_wheat.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_wheat > 5 & counter_wheat <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_wheat.Invoke((Action)delegate
                    {
                        textBox_bias_wheat.Text = (double.Parse(textBox_bias_wheat.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_wheat > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_wheat.Invoke((Action)delegate
                    {
                        textBox_bias_wheat.Text = (double.Parse(textBox_bias_wheat.Text) - 0.1).ToString("0.0");
                    });
                }
            }
        }


        #endregion

        #region barley
        private bool _runbarley = false;
        private void btn_plus_barley_MouseDown(object sender, MouseEventArgs e)
        {
            _runbarley = true;
            Thread readThread = new Thread(barley_Plus_Async);
            readThread.Start();
        }

        private void btn_plus_barley_MouseUp(object sender, MouseEventArgs e)
        {
            _runbarley = false;
        }

        public void barley_Plus_Async()
        {
            int counter_barley = 0;
            while (_runbarley)
            {
                counter_barley++;
                if (counter_barley <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_LongStickyRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongStickyRice.Text = (double.Parse(textBox_bias_LongStickyRice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_barley > 5 & counter_barley <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_LongStickyRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongStickyRice.Text = (double.Parse(textBox_bias_LongStickyRice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_barley > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_LongStickyRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongStickyRice.Text = (double.Parse(textBox_bias_LongStickyRice.Text) + 0.1).ToString("0.0");
                    });
                }
            }
        }


        private void btn_minus_barley_MouseDown(object sender, MouseEventArgs e)
        {
            _runbarley = true;
            Thread readThread = new Thread(barley_Minus_Async);
            readThread.Start();
        }

        private void btn_minus_barley_MouseUp(object sender, MouseEventArgs e)
        {
            _runbarley = false;
        }

        public void barley_Minus_Async()
        {
            int counter_barley = 0;
            while (_runbarley)
            {
                counter_barley++;
                if (counter_barley <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_LongStickyRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongStickyRice.Text = (double.Parse(textBox_bias_LongStickyRice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_barley > 5 & counter_barley <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_LongStickyRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongStickyRice.Text = (double.Parse(textBox_bias_LongStickyRice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_barley > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_LongStickyRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongStickyRice.Text = (double.Parse(textBox_bias_LongStickyRice.Text) - 0.1).ToString("0.0");
                    });
                }
            }
        }



        #endregion

        #region Soy
        private bool _runsoy = false;
        private void btn_plus_soy_MouseDown(object sender, MouseEventArgs e)
        {
            _runsoy = true;
            Thread readThread = new Thread(soy_Plus_Async);
            readThread.Start();
        }

        private void btn_plus_soy_MouseUp(object sender, MouseEventArgs e)
        {
            _runsoy = false;
        }
        public void soy_Plus_Async()
        {
            int counter_soy = 0;
            while (_runsoy)
            {
                counter_soy++;
                if (counter_soy <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_LongParboiledRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongParboiledRice.Text = (double.Parse(textBox_bias_LongParboiledRice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_soy > 5 & counter_soy <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_LongParboiledRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongParboiledRice.Text = (double.Parse(textBox_bias_LongParboiledRice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_soy > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_LongParboiledRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongParboiledRice.Text = (double.Parse(textBox_bias_LongParboiledRice.Text) + 0.1).ToString("0.0");
                    });
                }
            }
        }


        private void btn_minus_soy_MouseDown(object sender, MouseEventArgs e)
        {
            _runsoy = true;
            Thread readThread = new Thread(soy_Minus_Async);
            readThread.Start();
        }

        private void btn_minus_soy_MouseUp(object sender, MouseEventArgs e)
        {
            _runsoy = false;
        }


        public void soy_Minus_Async()
        {
            int counter_soy = 0;
            while (_runsoy)
            {
                counter_soy++;
                if (counter_soy <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_LongParboiledRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongParboiledRice.Text = (double.Parse(textBox_bias_LongParboiledRice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_soy > 5 & counter_soy <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_LongParboiledRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongParboiledRice.Text = (double.Parse(textBox_bias_LongParboiledRice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_soy > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_LongParboiledRice.Invoke((Action)delegate
                    {
                        textBox_bias_LongParboiledRice.Text = (double.Parse(textBox_bias_LongParboiledRice.Text) - 0.1).ToString("0.0");
                    });
                }
            }
        }


        #endregion

        #region polished
        private bool _runpolished = false;
        private void btn_plus_polished_MouseDown(object sender, MouseEventArgs e)
        {
            _runpolished = true;
            Thread readThread = new Thread(polished_Plus_Async);
            readThread.Start();
        }

        private void btn_plus_polished_MouseUp(object sender, MouseEventArgs e)
        {
            _runpolished = false;
        }

        public void polished_Plus_Async()
        {
            int counter_polished = 0;
            while (_runpolished)
            {
                counter_polished++;
                if (counter_polished <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_JasminePaddy.Invoke((Action)delegate
                    {
                        textBox_bias_JasminePaddy.Text = (double.Parse(textBox_bias_JasminePaddy.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_polished > 5 & counter_polished <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_JasminePaddy.Invoke((Action)delegate
                    {
                        textBox_bias_JasminePaddy.Text = (double.Parse(textBox_bias_JasminePaddy.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_polished > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_JasminePaddy.Invoke((Action)delegate
                    {
                        textBox_bias_JasminePaddy.Text = (double.Parse(textBox_bias_JasminePaddy.Text) + 0.1).ToString("0.0");
                    });
                }
            }
        }

        private void btn_minus_polished_MouseDown(object sender, MouseEventArgs e)
        {
            _runpolished = true;
            Thread readThread = new Thread(polished_Minus_Async);
            readThread.Start();
        }

        private void btn_minus_polished_MouseUp(object sender, MouseEventArgs e)
        {
            _runpolished = false;
        }

        public void polished_Minus_Async()
        {
            int counter_polished = 0;
            while (_runpolished)
            {
                counter_polished++;
                if (counter_polished <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_JasminePaddy.Invoke((Action)delegate
                    {
                        textBox_bias_JasminePaddy.Text = (double.Parse(textBox_bias_JasminePaddy.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_polished > 5 & counter_polished <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_JasminePaddy.Invoke((Action)delegate
                    {
                        textBox_bias_JasminePaddy.Text = (double.Parse(textBox_bias_JasminePaddy.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_polished > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_JasminePaddy.Invoke((Action)delegate
                    {
                        textBox_bias_JasminePaddy.Text = (double.Parse(textBox_bias_JasminePaddy.Text) - 0.1).ToString("0.0");
                    });
                }
            }
        }

        #endregion

        #region Corn
        private bool _runcorn = false;
        private void btn_plus_corn_MouseDown(object sender, MouseEventArgs e)
        {
            _runcorn = true;
            Thread readThread = new Thread(corn_Plus_Async);
            readThread.Start();
        }

        private void btn_plus_corn_MouseUp(object sender, MouseEventArgs e)
        {
            _runcorn = false;
        }


        public void corn_Plus_Async()
        {
            int counter_corn = 0;
            while (_runcorn)
            {
                counter_corn++;
                if (counter_corn <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_PeakADCount.Invoke((Action)delegate
                    {
                        textBox_bias_PeakADCount.Text = (double.Parse(textBox_bias_PeakADCount.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_corn > 5 & counter_corn <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_PeakADCount.Invoke((Action)delegate
                    {
                        textBox_bias_PeakADCount.Text = (double.Parse(textBox_bias_PeakADCount.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_corn > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_PeakADCount.Invoke((Action)delegate
                    {
                        textBox_bias_PeakADCount.Text = (double.Parse(textBox_bias_PeakADCount.Text) + 0.1).ToString("0.0");
                    });
                }
            }
        }

        private void btn_minus_corn_MouseDown(object sender, MouseEventArgs e)
        {
            _runcorn = true;
            Thread readThread = new Thread(corn_Minus_Async);
            readThread.Start();
        }

        private void btn_minus_corn_MouseUp(object sender, MouseEventArgs e)
        {
            _runcorn = false;
        }


        public void corn_Minus_Async()
        {
            int counter_corn = 0;
            while (_runcorn)
            {
                counter_corn++;
                if (counter_corn <= 6)
                {
                    Thread.Sleep(300);
                    textBox_bias_PeakADCount.Invoke((Action)delegate
                    {
                        textBox_bias_PeakADCount.Text = (double.Parse(textBox_bias_PeakADCount.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_corn > 5 & counter_corn <= 10)
                {
                    Thread.Sleep(200);
                    textBox_bias_PeakADCount.Invoke((Action)delegate
                    {
                        textBox_bias_PeakADCount.Text = (double.Parse(textBox_bias_PeakADCount.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_corn > 10)
                {
                    Thread.Sleep(50);
                    textBox_bias_PeakADCount.Invoke((Action)delegate
                    {
                        textBox_bias_PeakADCount.Text = (double.Parse(textBox_bias_PeakADCount.Text) - 0.1).ToString("0.0");
                    });
                }
            }
        }





        #endregion



        #region long milled rice

        private bool _runlongmilledrice = false;

        private void button_plus_bias_longmilledrice_MouseDown(object sender, MouseEventArgs e)
        {
            _runlongmilledrice = true;
            Thread readThread = new Thread(longmilledrice_Plus_Async);
            readThread.Start();
        }
        private void button_plus_bias_longmilledrice_MouseUp(object sender, MouseEventArgs e)
        {
            _runlongmilledrice = false;
        }
        public void longmilledrice_Plus_Async()
        {
            //DateTime Start = DateTime.Now();
            int counter_longmilledrice = 0;
            while (_runlongmilledrice)
            {
                counter_longmilledrice++;
                if (counter_longmilledrice <= 6)
                {
                    Thread.Sleep(300);
                    textBox_Bias_longmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_longmilledrice.Text = (double.Parse(textBox_Bias_longmilledrice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_longmilledrice > 5 & counter_longmilledrice <= 10)
                {
                    Thread.Sleep(200);
                    textBox_Bias_longmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_longmilledrice.Text = (double.Parse(textBox_Bias_longmilledrice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_longmilledrice > 10)
                {
                    Thread.Sleep(50);
                    textBox_Bias_longmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_longmilledrice.Text = (double.Parse(textBox_Bias_longmilledrice.Text) + 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }
        private void button_minus_bias_longmilledrice_MouseDown(object sender, MouseEventArgs e)
        {
            _runlongmilledrice = true;
            Thread readThread = new Thread(longmilledrice_Minus_Async);
            readThread.Start();
        }
        private void button_minus_bias_longmilledrice_MouseUp(object sender, MouseEventArgs e)
        {
            _runlongmilledrice = false;
        }
        public void longmilledrice_Minus_Async()
        {
            int counter_longmilledrice = 0;
            while (_runlongmilledrice)
            {
                counter_longmilledrice++;
                if (counter_longmilledrice <= 6)
                {
                    Thread.Sleep(300);
                    textBox_Bias_longmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_longmilledrice.Text = (double.Parse(textBox_Bias_longmilledrice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_longmilledrice > 5 & counter_longmilledrice <= 10)
                {
                    Thread.Sleep(200);
                    textBox_Bias_longmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_longmilledrice.Text = (double.Parse(textBox_Bias_longmilledrice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_longmilledrice > 10)
                {
                    Thread.Sleep(50);
                    textBox_Bias_longmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_longmilledrice.Text = (double.Parse(textBox_Bias_longmilledrice.Text) - 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }


        #endregion

        #region short milled rice

        private bool _runshortmilledrice = false;

        private void button_plus_bias_shortmilledrice_MouseDown(object sender, MouseEventArgs e)
        {
            _runshortmilledrice = true;
            Thread readThread = new Thread(shortmilledrice_Plus_Async);
            readThread.Start();
        }
        private void button_plus_bias_shortmilledrice_MouseUp(object sender, MouseEventArgs e)
        {
            _runshortmilledrice = false;
        }
        public void shortmilledrice_Plus_Async()
        {
            //DateTime Start = DateTime.Now();
            int counter_shortmilledrice = 0;
            while (_runshortmilledrice)
            {
                counter_shortmilledrice++;
                if (counter_shortmilledrice <= 6)
                {
                    Thread.Sleep(300);
                    textBox_Bias_shortmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_shortmilledrice.Text = (double.Parse(textBox_Bias_shortmilledrice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_shortmilledrice > 5 & counter_shortmilledrice <= 10)
                {
                    Thread.Sleep(200);
                    textBox_Bias_shortmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_shortmilledrice.Text = (double.Parse(textBox_Bias_shortmilledrice.Text) + 0.1).ToString("0.0");
                    });
                }

                else if (counter_shortmilledrice > 10)
                {
                    Thread.Sleep(50);
                    textBox_Bias_shortmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_shortmilledrice.Text = (double.Parse(textBox_Bias_shortmilledrice.Text) + 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }
        private void button_minus_bias_shortmilledrice_MouseDown(object sender, MouseEventArgs e)
        {
            _runshortmilledrice = true;
            Thread readThread = new Thread(shortmilledrice_Minus_Async);
            readThread.Start();
        }
        private void button_minus_bias_shortmilledrice_MouseUp(object sender, MouseEventArgs e)
        {
            _runshortmilledrice = false;
        }
        public void shortmilledrice_Minus_Async()
        {
            int counter_shortmilledrice = 0;
            while (_runshortmilledrice)
            {
                counter_shortmilledrice++;
                if (counter_shortmilledrice <= 6)
                {
                    Thread.Sleep(300);
                    textBox_Bias_shortmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_shortmilledrice.Text = (double.Parse(textBox_Bias_shortmilledrice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_shortmilledrice > 5 & counter_shortmilledrice <= 10)
                {
                    Thread.Sleep(200);
                    textBox_Bias_shortmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_shortmilledrice.Text = (double.Parse(textBox_Bias_shortmilledrice.Text) - 0.1).ToString("0.0");
                    });
                }

                else if (counter_shortmilledrice > 10)
                {
                    Thread.Sleep(50);
                    textBox_Bias_shortmilledrice.Invoke((Action)delegate
                    {
                        textBox_Bias_shortmilledrice.Text = (double.Parse(textBox_Bias_shortmilledrice.Text) - 0.1).ToString("0.0");
                    });
                }


                //You actions
            }
        }


        #endregion


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_bias_apply_Click_1(object sender, EventArgs e)
        {
            if (double.Parse(textBox_Bias_ShortPaddy.Text) < -15 || double.Parse(textBox_Bias_ShortPaddy.Text) > 15)
            {
                MessageBox.Show(var_label1 + "Range is -15% to 15%");
            }
            else if (double.Parse(textBox_bias_LongPaddy.Text) < -15 || double.Parse(textBox_bias_LongPaddy.Text) > 15)
            {
                MessageBox.Show(var_label2 + "Range is -15% to 15%");
            }
            else if (double.Parse(textBox_bias_wheat.Text) < -15 || double.Parse(textBox_bias_wheat.Text) > 15)
            {
                MessageBox.Show(var_label3 + "Range is -15% to 15%");
            }

            else if (double.Parse(textBox_bias_JasminePaddy.Text) < -15 || double.Parse(textBox_bias_JasminePaddy.Text) > 15)
            {
                MessageBox.Show(var_label4 + "Range is -15% to 15%");
            }

            else if (double.Parse(textBox_bias_LongStickyRice.Text) < -15 || double.Parse(textBox_bias_LongStickyRice.Text) > 15)
            {
                MessageBox.Show(var_label5 + "Range is -15% to 15%");
            }

            else if (double.Parse(textBox_bias_LongParboiledRice.Text) < -15 || double.Parse(textBox_bias_LongParboiledRice.Text) > 15)
            {
                MessageBox.Show(var_label6 + "Range is -15% to 15%");
            }

            else if (double.Parse(textBox_bias_PeakADCount.Text) < -15 || double.Parse(textBox_bias_PeakADCount.Text) > 15)
            {
                MessageBox.Show(var_label7 + "Range is -15% to 15%");
            }

            
            
            else
            {
                //double sql_value_paddy = double.Parse(textBox_Bias_Paddy.Text);
                /*
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "short_paddy", textBox_Bias_ShortPaddy.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "long_paddy", textBox_bias_LongPaddy.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "wheat", textBox_bias_wheat.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "long_sticky_paddy", textBox_bias_LongStickyRice.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "long_parbolied_rice", textBox_bias_LongParboiledRice.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "peakadcount", textBox_bias_PeakADCount.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "jasmine_paddy", textBox_bias_JasminePaddy.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "long_milled_rice", textBox_Bias_longmilledrice.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), "short_milled_rice", textBox_Bias_shortmilledrice.Text);

                 */
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label1.ToLower().Replace(" ","_"), textBox_Bias_ShortPaddy.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label2.ToLower().Replace(" ", "_"), textBox_bias_LongPaddy.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label3.ToLower().Replace(" ", "_"), textBox_bias_wheat.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label4.ToLower().Replace(" ", "_"), textBox_bias_LongStickyRice.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label5.ToLower().Replace(" ", "_"), textBox_bias_LongParboiledRice.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label6.ToLower().Replace(" ", "_"), textBox_bias_PeakADCount.Text);
                Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label7.ToLower().Replace(" ", "_"), textBox_bias_JasminePaddy.Text);
                
                
                if (product_type == 3)
                {
                    if (double.Parse(textBox_Bias_longmilledrice.Text) < -15 || double.Parse(textBox_Bias_longmilledrice.Text) > 15)
                {
                        MessageBox.Show(var_label8 + "Range is -15% to 15%");
                    }

                    else if (double.Parse(textBox_Bias_shortmilledrice.Text) < -15 || double.Parse(textBox_Bias_shortmilledrice.Text) > 15)
                    {
                        MessageBox.Show(var_label9 + "Range is -15% to 15%");
                    }
                    else
                    {
                        Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label8.ToLower().Replace(" ", "_"), textBox_Bias_longmilledrice.Text);
                        Sensor_input_Helper.Update_DataConfig(Sensor_input_Helper.GetLocalIPAddress(), var_label9.ToLower().Replace(" ", "_"), textBox_Bias_shortmilledrice.Text);

                    }
                }


                this.Close();
            }

        }

        private void Button_Close_BiasNew(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox_bias_Polished_Rice_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }



}
