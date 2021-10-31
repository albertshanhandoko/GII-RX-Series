using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControllerPage.Constant;
using ControllerPage.Helper;
using ControllerPage.Library;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Timers;
using System.IO;
using System.Security.Permissions;
using System.Numerics;
using System.Globalization;

namespace ControllerPage
{
    public partial class Form1 : Form
    {
        SerialPort mySerialPort;
        static string application_name = "GX_MX_001_Controller";

      

        // Parameter Input
        int delay;
        int TotalInterval;
        int running_time_fixed;
        TimeSpan Time_Dif;
        string ResultGrain;
        string ResultMeasure;
        bool temp_cond;
        bool thereshold_param;
        double therehold_max;
        double thereshold_min;
        double thereshold_min_counter;
        double thereshold_max_counter;

        List<String> List_Error_code = new List<string> { };
        //System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();
        System.Timers.Timer MyTimer = new System.Timers.Timer();
        System.Timers.Timer Timer_5min_StopCheck = new System.Timers.Timer();

        public Thread check_thread;
        public Thread checktemp_thread;
        public Thread start_thread;
        public Thread stop_5min_thread;

        bool bool_check_error = false;
        bool bool_checksum_error = false;
        bool bool_stop_click = false;
        int blink_timer;

        // Parameter Looping Sensor
        double bias_value;
        int current_interval_reset;
        int current_interval;
        int counter_data = 0;
        int counter_data_reset = 0;
        bool start_next_cond;
        bool aggregate_cond;
        bool stat_continue;
        bool fixed_time_timer_stop;
        List<data_measure_2> Data_Measure_Result = new List<data_measure_2> { };
        List<data_measure_2> Data_Avg_Result = new List<data_measure_2> { };

        data_measure_2 Data_Measure_Current;
        data_measure_2 Data_Avg_Current;
        int timer_counter = 0;
        float total_current_Average;
        float total_average;
        int finish_measurement = 0;
        DateTime FixedTime_start;
        DateTime FixedTime_Finish;
        DateTime FixedTime_Finish_timer;
        DateTime start_5min_check;
        DateTime start_5min_Running;

        //database parameter
        int batch_id;
        bool checkcommand;

        // other
        //Thread check_thread = new Thread(Check_Thread);


  

        public Form1()
        {
            InitializeComponent();
            MyTimer.Elapsed += new ElapsedEventHandler(MyTimer_Tick);
            MyTimer.Interval = (1000);
            label1.Text = Global.GlobalVar1;
            textBox10.Text = Global.GlobalVar2;
            textBox14.Text = Global.GlobalVar3;
            textBox15.Text = Global.GlobalVar4;
            textBox6.Text = Global.GlobalVar5;
            textBox7.Text = Global.GlobalVar6;
            textBox8.Text = Global.GlobalVar7;
            textBox9.Text = Global.GlobalVar8;
            label4.Text = Global.GlobalVar9;
            label_ipaddress.Text = Global.GlobalVar10;
            textBox4.Text= Global.GlobalVar11;
            textBox11.Text = Global.GlobalVar12;
            data_initiation_input();
            ButtonProduct.Enabled = true;

        }

        #region Button_Other
        private void button1_Click(object sender, EventArgs e)
        {
            Sensor_input_Helper.Command_CheckData(mySerialPort);

        }
        private void button2_Click_3(object sender, EventArgs e)
        {
            this.Invalidate();
            this.Refresh();
        }
        private void button3_Click_2(object sender, EventArgs e)
        {
            //Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);
            //Sensor_input_Helper.changeip();
            //Form1_Load(e, e);// 'Load everything in your form, load event again
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Sensor_input_Helper.Command_Write(mySerialPort, "10192\r");
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            Sensor_input_Helper.Command_Write(mySerialPort, "22094\r");
        }
        #endregion


        #region Button_Func


        private void Button_Mode_Click(object sender, EventArgs e)
        {
            using (var form = new FormMode())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.ModeSelection;            //values preserved after close
                    //Do something here with these values
                    string display_val = val.Replace("_", " ");
                    Button_Mode.Text = display_val;

                }
            }
            if (Button_Mode.Text.ToLower() == "fixed time")
            {

                Console.WriteLine("Time mode");
                ButtonProduct.Enabled = true;
                ButtonNumInterval.Enabled = false;
                ButtonNumInterval.Text = string.Empty;
                ButtonNumPcs.Enabled = false;
                ButtonNumPcs.Text = string.Empty;
                ButtonWaitingTime.Enabled = true;
                ButtonWaitingTime.Text = string.Empty;
                textBox9.Text = "Running Time";

            }
            else if (Button_Mode.Text.ToLower() == "fixed pieces")
            {

                Console.WriteLine("Time mode");
                ButtonProduct.Enabled = true;
                ButtonNumInterval.Enabled = false;
                ButtonNumInterval.Text = string.Empty;
                ButtonNumPcs.Enabled = true;
                //ButtonNumPcs.Text = string.Empty;
                ButtonWaitingTime.Enabled = false;
                ButtonWaitingTime.Text = string.Empty;

            }

            else if (Button_Mode.Text.ToLower() == "interval")
            {
                ButtonProduct.Enabled = true;
                ButtonNumInterval.Enabled = true;
                ButtonNumPcs.Enabled = true;
                ButtonWaitingTime.Enabled = true;
                ButtonWaitingTime.Text = string.Empty;
                textBox9.Text = "Int. Waiting Time";

            }
            else
            {
                MessageBox.Show("Please pick Mode", application_name);
            }



        }
        private void Button_Interface_Click(object sender, EventArgs e)
        {
            using (var form = new FormInterface())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.Interfaceselection;            //values preserved after close
                    //Do something here with these values
                    string display_val = val.Replace("_", " ");


                    Button_Interface.Text = display_val;
                    //Button_Interface.Text = "COM2"; //testing only
                }
            }
            if (Button_Interface.Text == "RS-232")
            {
                mySerialPort = new SerialPort("/dev/ttyAMA0"); //232
                //mySerialPort = new SerialPort("COM2"); //testing

            }
            else if (Button_Interface.Text == "RS-485")
            {
                mySerialPort = new SerialPort("/dev/ttyS0"); //485
            }
            else
            {
                MessageBox.Show("Please Pick Interface", application_name);
            }


        }
        private void ButtonIPSet_Click(object sender, EventArgs e)
        {
            using (var form = new FormIPSet())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ButtonIPSet.Text = FormIPSet.combobox_selectedItem_ipsetting;
                }
            }
        }

        private void button_Product_Click(object sender, EventArgs e)
        {
            //someControlOnForm1.Text = form2.prod;

            //            using (var form = new FormProductselection_V010())
            using (var form = new FormProductselection())

            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string val = form.Productselection;            //values preserved after close
                    //Do something here with these values
                    string display_val = val.Replace("_", " ");
                    ButtonProduct.Text = display_val;

                }
            }

        }
        private void button_NumInterval_Click(object sender, EventArgs e)
        {
            using (var form = new FormNumberinterval())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ButtonNumInterval.Text = FormNumberinterval.combobox_selectedItem_number_Interval;
                }
            }

        }
        private void button_NumPerPcs_Click(object sender, EventArgs e)
        {
            using (var form = new FormNumberpcsinterval())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ButtonNumPcs.Text = FormNumberpcsinterval.combobox_selectedItem_number_PerPCS;
                }
            }
        }
        private void button_Time_Click(object sender, EventArgs e)
        {
            if (Button_Mode.Text.ToLower() == "fixed time")
            {

                using (var form = new FormFixedTime())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ButtonWaitingTime.Text = FormFixedTime.combobox_selectedItem_WaitingTime;

                    }
                }

            }
            else
            {
                using (var form = new FormIntervalTime())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ButtonWaitingTime.Text = FormIntervalTime.combobox_selectedItem_WaitingTime;

                    }
                }

            }


        }
        private void button_Option_Click(object sender, EventArgs e)
        {
            this.Hide();
            
            //Form2_old F2 = new Form2_old();
            FormOptions FormOption_open = new FormOptions();
            //this.Hide();
            FormOption_open.ShowDialog();

            //this.Show();
        }
        private void Btn_Stop_Click(object sender, EventArgs e)
        {
            Sensor_input_Helper.Command_Stop(mySerialPort);
            bool_stop_click = true;
            MyTimer.Enabled = false;
            //MyTimer.Stop();
        }
        private void Btn_Check_Click(object sender, EventArgs e)
        {
            Btn_Check.Enabled = false;
            if (Button_Interface.Text != "RS-232" && Button_Interface.Text != "RS-485")
            {
                MessageBox.Show("Please Pick Interface " + Button_Interface.Text, application_name);
            }
            else
            {
                data_initiation_input();
                

                #region Reset Sensor Connection
                if (!mySerialPort.IsOpen)
                {
                    SensorHelper_2.OpenCon_Port(mySerialPort, 1200);
                    Thread.Sleep(30);
                }
                else
                {
                    mySerialPort.Close();
                    Thread.Sleep(100);
                    SensorHelper_2.OpenCon_Port(mySerialPort, 1200);
                    Thread.Sleep(30);
                }
                #endregion
                try
                {
                
                    bool check_connect_result;
                    check_connect_result = check_connection_sensor();
                    //check_connect_result = true;

                    if (check_connect_result)
                    {
                        check_thread = new Thread(Check_Thread);
                        check_thread.Start();

                    }
                    else
                    {
                        Console.Write("Error 101, else");
                    }

                }
                catch (Exception ex)
                {
                    batch_id = Sensor_input_Helper.MySql_Insert_Batch(Sensor_input_Helper.GetLocalIPAddress()
                                , "0"
                                , 0
                                , "0"
                                , 0
                                , "0")
                                ;
                    MessageBox.Show("Error 001 - There is no signal between sensor and controller.");
                    Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id, "001");
                    Console.WriteLine(ex.Message);
                }

            }

        }
        private void button_start_Click(object sender, EventArgs e)
        {
            bool check_connect_result = false;
            try
            {
                check_connect_result = check_connection_sensor();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error 001 - Connection to sensor failed");
                Console.WriteLine(ex.Message);
            }

            data_cleansing();
            bool check_start = Start_Validation();

            if (check_start && check_connect_result)
            {

                Temp_TextBox.Text = "";
                ///*
                string product_text;

                #region Bahasa to english

                #endregion
                if (ButtonProduct.Text== "Corn High"|| ButtonProduct.Text == "Jagung Tinggi")
                {
                    product_text = "Brown_Rice";
                }
                else if (ButtonProduct.Text == "Corn Medium"|| ButtonProduct.Text =="Jagung Medium")
                {
                    product_text = "Wheat";
                }
                else if (ButtonProduct.Text == "Corn Low"|| ButtonProduct.Text =="Jagung Rendah")
                {
                    product_text = "Corn";
                }
                else if (ButtonProduct.Text == "Padi")
                {
                    product_text = "Paddy";
                }
                else if (ButtonProduct.Text == "Kedelai")
                {
                    product_text = "Soy";
                }
                else if (ButtonProduct.Text == "Beras Poles")
                {
                    product_text = "Polished_Rice";
                }
                else
                {
                    product_text = ButtonProduct.Text.Replace(" ", "_");
                }
                //*/
                //string product_text= ButtonProduct.Text.Replace(" ", "_");
                List<SQL_Data_Config> current_config = Sensor_input_Helper.MySql_Get_DataConfig(Sensor_input_Helper.GetLocalIPAddress());
                var Product_var = current_config.Where(config => config.Config_Param == product_text.ToLower());
                double Product_value = (Product_var.Select(p => p.Config_Value).ToArray()).First();

                var TheresholdMax_var = current_config.Where(config => config.Config_Param == "Thereshold_Max");
                therehold_max = (TheresholdMax_var.Select(p => p.Config_Value).ToArray()).First();

                var TheresholdMin_var = current_config.Where(config => config.Config_Param == "Thereshold_Min");
                thereshold_min = (TheresholdMin_var.Select(p => p.Config_Value).ToArray()).First();

                var TheresholdEnable_var = current_config.Where(config => config.Config_Param == "Thereshold_Enable");
                double TheresholdEnable_value = (TheresholdEnable_var.Select(p => p.Config_Value).ToArray()).First();


                if (TheresholdEnable_value == 1)
                {
                    thereshold_param = true;

                }
                else
                {
                    thereshold_param = false;
                }


                //Thereshold_Max Thereshold_Min Thereshold_Enable

                bias_value = Product_value;

                if (Temp_TextBox.Text == "" || String.IsNullOrEmpty(Temp_TextBox.Text))
                {
                    Sensor_input_Helper.Command_CheckTemp(mySerialPort);
                    //string result_temp = "29";
                    string result_temp = CheckTemp();
                }
                else
                {
                    Console.WriteLine("textbox alread filled");
                }
                int jumlahpieces = 0;
                int number;
                if (Int32.TryParse(ButtonNumPcs.Text, out number))
                {
                    jumlahpieces = number;
                }

                Thread.Sleep(500);
                
                batch_id = Sensor_input_Helper.MySql_Insert_Batch(Sensor_input_Helper.GetLocalIPAddress()
                    , ButtonProduct.Text
                    , TotalInterval
                    , delay.ToString()
                    , jumlahpieces
                    , Temp_TextBox.Text)
                    ;



                Console.WriteLine(ResultGrain);
                Console.WriteLine(ResultMeasure);
                Sensor_input_Helper.Command_Stop(mySerialPort);
                Thread.Sleep(2500);
                Console.WriteLine("Stop");
                Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
                Thread.Sleep(1000);
                Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);

                Console.WriteLine("Start Sequence");
                current_interval = 0;
                current_interval_reset = 0;
                Curr_Interval_TextBox.Text = (current_interval + 1).ToString();

                stat_continue = true;
                Btn_Start.Enabled = false;
                Btn_Stop.Enabled = true;
                Btn_CheckTemp.Enabled = false;
                Btn_Check.Enabled = false;


                Button_Mode.Enabled = false;
                
                textBox_Sensor_Status.Text = "Running";
                textBox_Sensor_Status.ForeColor = Color.Green;
                // check during start
                mySerialPort.ReadTimeout = 60 * 1000 * 5;// in miliseconds

                Thread readThread;
                if (Button_Mode.Text.ToLower() == "fixed time")
                {
                    FixedTime_start = DateTime.Now;
                    fixed_time_timer_stop = true;
                    start_thread = new Thread(Read_FixedTime_Thread);
                    start_thread.Start();

                    //readThread = new Thread(Read_FixedTime);
                    //readThread.Start();
                }
                else if (Button_Mode.Text.ToLower() == "fixed pieces")
                {
                    //readThread = new Thread(Read_FixedPieces);
                    //readThread.Start();

                    start_thread = new Thread(Read_FixedPieces_Thread);
                    start_thread.Start();

                }
                else if (Button_Mode.Text.ToLower() == "interval")
                {
                    //readThread = new Thread(Read_Interval);
                    //readThread.Start();
                    start_thread = new Thread(Read_Interval_Thread);
                    start_thread.Start();

                }
                else
                {
                    MessageBox.Show("Wrong Picked on mode");
                }
            }


        }
        private void button_CheckTemp_Click(object sender, EventArgs e)
        {
            Sensor_input_Helper.Command_CheckTemp(mySerialPort);
            Thread.Sleep(2000);

            try
            {
                checktemp_thread = new Thread(CheckTemp_Thread);
                checktemp_thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sensor Failed to get temp");
                Console.WriteLine(ex.Message);
            }


        }

        #endregion

        #region Function


        private List<string> GetWords(string text)
        {
            Regex reg = new Regex("[a-zA-Z0-9]");
            string Word = "";
            char[] ca = text.ToCharArray();
            List<string> characters = new List<string>();
            for (int i = 0; i < ca.Length; i++)
            {
                char c = ca[i];
                if (c > 65535)
                {
                    continue;
                }
                if (char.IsHighSurrogate(c))
                {
                    i++;
                    characters.Add(new string(new[] { c, ca[i] }));
                }
                else
                {
                    if (reg.Match(c.ToString()).Success || c.ToString() == "/")
                    {
                        Word = Word + c.ToString();
                        //characters.Add(new string(new[] { c }));
                    }
                    else if (c.ToString() == " ")
                    {
                        if (Word.Length > 0)
                            characters.Add(Word);
                        Word = "";
                    }
                    else
                    {
                        if (Word.Length > 0)
                            characters.Add(Word);
                        Word = "";
                    }

                }

            }
            return characters;
        }

        private bool check_connection_sensor()
        {
            bool check_result = false;
            try
            {
                mySerialPort.ReadTimeout = 10 * 1000;// in miliseconds
                bool check_start = true;
                string readStr = string.Empty;
                byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                int readLen;

                readBuffer = new byte[mySerialPort.ReadBufferSize];
                readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                mySerialPort.DiscardInBuffer();
                mySerialPort.DiscardOutBuffer();

                int check_counter = 0;

                while (check_start == true && check_counter <= 5)
                {
                    Thread.Sleep(2000);// this solves the problem
                    readBuffer = new byte[mySerialPort.ReadBufferSize];
                    readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);

                    if (readStr.ToLower().Contains("r"))
                    {
                        check_start = false;
                        check_result = true;
                        Console.WriteLine("Check Result True", application_name);
                    }
                    check_counter++;
                }
                Console.WriteLine("nilai check result adalah: " + check_result.ToString(), application_name);
                


            }
            catch (Exception ex)
            {
                next_action_button(true);
                Thread.Sleep(2000);
                MessageBox.Show("Error 001 - Failed to connect to sensor.");
                Console.WriteLine(ex.Message);
            }
            return check_result;
        }

        private void Button_Mode_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        private void data_cleansing()
        {
            // Parameter Di Layar
            /*
            ButtonProduct.Text = "";
            ButtonNumInterval.Text = "";
            ButtonNumPcs.Text = "";
            ButtonWaitingTime.Text = "";
            */
            Curr_Interval_TextBox.Text = "";
            Curr_Kernel_TextBox.Text = "";
            Current_Avg_TextBox.Text = "";
            Curr_Measure_TextBox.Text = "";
            Temp_TextBox.Text = "";
            textBox_theresholdmax.Text = "";
            textBox_theresholdmin.Text = "";


            // Parameter Input
            delay = 0;
            TotalInterval = 0;
            counter_data = 0;
            counter_data_reset = 0;
            ResultGrain = null;
            ResultMeasure = null;
            temp_cond = false;
            //MyTimer = 
            // selalu pake Sytem.Timers.Timer

            thereshold_param = false;
            blink_timer = 0;

            // Parameter Looping Sensor
            current_interval = 0;
            current_interval_reset = 0;
            start_next_cond = false;
            aggregate_cond = false;
            stat_continue = false;
            Data_Measure_Result = new List<data_measure_2> { };
            Data_Avg_Result = new List<data_measure_2> { };
            Data_Measure_Current = null;
            Data_Avg_Current = null;
            timer_counter = 0;
            total_current_Average = 0;
            total_average = 0;
            finish_measurement = 0;
            fixed_time_timer_stop = false;
            bool_check_error = false;
            bool_stop_click = false;
            bool_checksum_error = false;
            thereshold_max_counter = 0;
            thereshold_min_counter = 0;

            //database parameter

            // Button input


        }
        private void data_initiation_input()
        {

            ButtonIPSet.Text = Sensor_input_Helper.GetLocalIPAddress().Last().ToString() ;
            Button_Mode.Text = "MODE";

            // Mode cannot be clicked
            Button_Mode.Enabled = false;

            // at the start button cannot be clicked
            ButtonProduct.Enabled = false;
            ButtonNumPcs.Enabled = false;
            ButtonNumInterval.Enabled = false;

            ButtonWaitingTime.Enabled = false;
            // diaktifan ketika ganti combobox_moe

            
            Btn_Start.Enabled = false;
            Btn_Stop.Enabled = false;
            Btn_CheckTemp.Enabled = false;
            // diaktifan ketika click check

            //online and no online
            textBox_Sensor_Status.Text = "Offline";
            textBox_Sensor_Status.ForeColor = Color.Red;

            // TImer


            Timer_5min_StopCheck.Elapsed += new ElapsedEventHandler(MyTimer_CheckStop_Tick);
            Timer_5min_StopCheck.Interval = (60000); // testing
            List_Error_code = Sensor_input_Helper.get_List_Error_Code();

            //textBox_sensornumber.Text = "SENSOR " + (Sensor_input_Helper.GetLocalIPAddress()).Last().ToString();

            //label_ipaddress.Text = "SENSOR " + (Sensor_input_Helper.GetLocalIPAddress()).Last().ToString();
            //label_ipaddress.Text = "Sensor Number";

            // Thershold Counter
            thereshold_max_counter = 0;
            thereshold_min_counter = 0;
            //textBox_theresholdMaxCounter.Text = thereshold_max_counter.ToString();
            //textBox_theresholdMinCounter.Text = thereshold_min_counter.ToString();


        }

        private bool Start_Validation()
        {
            bool isvalid = false;
            if (Button_Mode.Text.ToLower() == "mode")
            {
                MessageBox.Show("PLease Enter Controller Mode", application_name);
                isvalid = false;
            }

            else if (Button_Mode.Text.ToLower() == "fixed time")
            {
                if (ButtonProduct.Text == "")
                {
                    MessageBox.Show("PLease Enter Product", application_name);
                }
                else if (ButtonWaitingTime.Text == "")
                {
                    MessageBox.Show("PLease Enter Running Time", application_name);
                }
                else
                {
                    // Running Time Interval
                    var result = Sensor_input_Helper.GetEnumValueFromDescription<Running_Time>(ButtonWaitingTime.Text);
                    running_time_fixed = ((int)(result)) * 60 / 1000;

                    // Number Grain Maximal
                    ResultGrain = "12598\r";
                    //ResultGrain = "10192\r";// testing only

                    // Product
                    ///*
                    string combox_typemeasure;
                    if (ButtonProduct.Text == "Corn High" || ButtonProduct.Text == "Jagung Tinggi")
                    {
                        combox_typemeasure = "Brown_Rice";
                    }
                    else if (ButtonProduct.Text == "Corn Medium" || ButtonProduct.Text == "Jagung Medium")
                    {
                        combox_typemeasure = "Wheat";
                    }
                    else if (ButtonProduct.Text == "Corn Low" || ButtonProduct.Text == "Jagung Rendah")
                    {
                        combox_typemeasure = "Corn";
                    }
                    else if (ButtonProduct.Text == "Padi")
                    {
                        combox_typemeasure = "Paddy";
                    }
                    else if (ButtonProduct.Text == "Kedelai")
                    {
                        combox_typemeasure = "Soy";
                    }
                    else if (ButtonProduct.Text == "Beras Poles")
                    {
                        combox_typemeasure = "Polished_Rice";
                    }
                    else
                    {
                        combox_typemeasure = ButtonProduct.Text.Replace(" ", "_");
                    }
                    //*/

                    //string combox_typemeasure = ButtonProduct.Text;
                    //combox_typemeasure = combox_typemeasure.Replace(" ", "_");
                    TypeOfMeasure enum_typemeasure = (TypeOfMeasure)Enum.Parse(typeof(TypeOfMeasure), combox_typemeasure);
                    ResultMeasure = Sensor_input_Helper.GetDescription(enum_typemeasure);

                    isvalid = true;

                    //running_time_fixed = 120;// in seconds. //testing only
                }

            }
            else if (Button_Mode.Text.ToLower() == "interval")
            {
                if (ButtonProduct.Text == "")
                {
                    MessageBox.Show("PLease Enter Product", application_name);
                }
                else if (ButtonNumInterval.Text == "")
                {
                    MessageBox.Show("PLease Enter Number Interval", application_name);
                }
                else if (ButtonNumPcs.Text == "")
                {
                    MessageBox.Show("PLease Enter Number of Pieces", application_name);
                }
                else if (ButtonWaitingTime.Text == "")
                {
                    MessageBox.Show("PLease Enter Waiting Time", application_name);
                }
                else
                {

                    // Waiting Time Interval
                    var result = Sensor_input_Helper.GetEnumValueFromDescription<Time_Interval>(ButtonWaitingTime.Text);
                    delay = ((int)(result)) * 60;

                    // Total Interval
                    TotalInterval = int.Parse(ButtonNumInterval.Text.ToString());

                    // Total Number Per Pieces
                    number_grain enum_numgrain = (number_grain)Enum.Parse(typeof(number_grain), ButtonNumPcs.Text);
                    ResultGrain = Sensor_input_Helper.GetDescription(enum_numgrain);

                    // Product

                    ///*
                    string combox_typemeasure;
                    if (ButtonProduct.Text == "Corn High" || ButtonProduct.Text == "Jagung Tinggi")
                    {
                        combox_typemeasure = "Brown_Rice";
                    }
                    else if (ButtonProduct.Text == "Corn Medium" || ButtonProduct.Text == "Jagung Medium")
                    {
                        combox_typemeasure = "Wheat";
                    }
                    else if (ButtonProduct.Text == "Corn Low" || ButtonProduct.Text == "Jagung Rendah")
                    {
                        combox_typemeasure = "Corn";
                    }
                    else if (ButtonProduct.Text == "Padi")
                    {
                        combox_typemeasure = "Paddy";
                    }
                    else if (ButtonProduct.Text == "Kedelai")
                    {
                        combox_typemeasure = "Soy";
                    }
                    else if (ButtonProduct.Text == "Beras Poles")
                    {
                        combox_typemeasure = "Polished_Rice";
                    }
                    else
                    {
                        combox_typemeasure = ButtonProduct.Text.Replace(" ", "_");
                    }
                    //*/

                    //string combox_typemeasure = ButtonProduct.Text;
                    //combox_typemeasure = combox_typemeasure.Replace(" ", "_");
                    TypeOfMeasure enum_typemeasure = (TypeOfMeasure)Enum.Parse(typeof(TypeOfMeasure), combox_typemeasure);
                    ResultMeasure = Sensor_input_Helper.GetDescription(enum_typemeasure);

                    isvalid = true;

                }

            }
            else if (Button_Mode.Text.ToLower() == "fixed pieces")
            {
                if (ButtonProduct.Text == "")
                {
                    MessageBox.Show("PLease Enter Product", application_name);
                }
                else if (ButtonNumPcs.Text == "")
                {
                    MessageBox.Show("PLease Enter Number of Pieces", application_name);
                }
                else
                {

                    // No Waiting Time Interval
                    //var result = Sensor_input_Helper.GetEnumValueFromDescription<Time_Interval>(ButtonWaitingTime.Text);
                    //delay = ((int)(result)) * 60;

                    // Total Interval
                    TotalInterval = 1;

                    // Total Number Per Pieces
                    number_grain enum_numgrain = (number_grain)Enum.Parse(typeof(number_grain), ButtonNumPcs.Text);
                    ResultGrain = Sensor_input_Helper.GetDescription(enum_numgrain);

                    // Product
                    ///*
                    string combox_typemeasure;
                    if (ButtonProduct.Text == "Corn High" || ButtonProduct.Text == "Jagung Tinggi")
                    {
                        combox_typemeasure = "Brown_Rice";
                    }
                    else if (ButtonProduct.Text == "Corn Medium" || ButtonProduct.Text == "Jagung Medium")
                    {
                        combox_typemeasure = "Wheat";
                    }
                    else if (ButtonProduct.Text == "Corn Low" || ButtonProduct.Text == "Jagung Rendah")
                    {
                        combox_typemeasure = "Corn";
                    }
                    else if (ButtonProduct.Text == "Padi")
                    {
                        combox_typemeasure = "Paddy";
                    }
                    else if (ButtonProduct.Text == "Kedelai")
                    {
                        combox_typemeasure = "Soy";
                    }
                    else if (ButtonProduct.Text == "Beras Poles")
                    {
                        combox_typemeasure = "Polished_Rice";
                    }
                    else
                    {
                        combox_typemeasure = ButtonProduct.Text.Replace(" ", "_");
                    }
                    //*/

                    //string combox_typemeasure = ButtonProduct.Text;
                    //combox_typemeasure = combox_typemeasure.Replace(" ", "_");
                    TypeOfMeasure enum_typemeasure = (TypeOfMeasure)Enum.Parse(typeof(TypeOfMeasure), combox_typemeasure);
                    ResultMeasure = Sensor_input_Helper.GetDescription(enum_typemeasure);

                    isvalid = true;
                }

            }


            return isvalid;

        }
        private string CheckTemp()
        {
            temp_cond = true;
            string Result_Parsing = "";
            DateTime check_temp_5min_start = DateTime.Now;
            while (temp_cond)
            {
                try
                {
                    Thread.Sleep(4000);// this solves the problem
                    byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                    int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                    string readStr = string.Empty;

                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                    readStr = readStr.Trim();
                    Console.WriteLine("ReadStr adalah: " + readStr);

                    string[] charactersToReplace = new string[] { @"r" };
                    foreach (string s in charactersToReplace)
                    {
                        readStr = readStr.Replace(s, "");
                    }

                    char[] delimiter_r = { '\r' };
                    string[] Measures_With_U = readStr.Split(delimiter_r);

                    foreach (string measure in Measures_With_U)
                    {
                        bool isDigitPresent = measure.Any(c => char.IsDigit(c));
                        if (isDigitPresent == true)
                        {
                            Result_Parsing = measure;
                        }

                    }
                    //Result_Parsing = Measures_With_U.Last();
                    int n = 0;
                    bool check_if_number = int.TryParse(Result_Parsing, out n);
                    if (Result_Parsing == "1000")
                    {
                        check_Error("1000");
                        next_action_button(true);
                        temp_cond = false;

                    }
                    else if (Result_Parsing == "1600")
                    {
                        check_Error("1600");
                        next_action_button(true);
                        temp_cond = false;

                    }

                    else if (
                        Result_Parsing.Length == 4
                        && Result_Parsing.Any(c => char.IsDigit(c))
                        //&& check_if_number == true
                        )
                    {
                        Result_Parsing = Result_Parsing.Substring(Result_Parsing.Length - 3);
                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                    , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                        Temp_TextBox.Invoke((Action)delegate
                        {
                            Temp_TextBox.Text = Result_Parsing;
                        });
                        Sensor_input_Helper.Command_Stop(mySerialPort);
                        temp_cond = false;
                    }
                    else
                    {
                        Console.WriteLine("this is checktemp else: " + Result_Parsing);
                    }

                }
                catch (TimeoutException ex)
                {
                    MessageBox.Show(this, "Error 011 - no message during checking for 5 mins");
                    //bool error = true;
                    next_action_button(true);
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Console.WriteLine(ex);
                    //return "";
                }
            }

            Console.WriteLine("Finsih_Check_Temp");
            return Result_Parsing;
            //Sensor_input_Helper.Command_Stop(mySerialPort);
        }
        private bool check_db_connection()
        {
            bool check_dbcon = false;
            check_dbcon = Sensor_input_Helper.Check_MySQL_Connect();
            if (check_dbcon == false)
            {
                MessageBox.Show(this, "Database is not connected");
            }
            return check_dbcon;

        }

        private void next_action_button(bool bool_check_error_next)
        {
            if (!bool_check_error_next)
            {
                // klo ga error
                MyTimer.Enabled = false;
                //MyTimer.Stop();

                Btn_Start.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Btn_Start.Enabled = true;
                });
                Btn_Stop.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Btn_Stop.Enabled = true;
                });
                Button_Mode.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Button_Mode.Enabled = true;
                });
                Btn_CheckTemp.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Btn_CheckTemp.Enabled = true;
                });
                Btn_Check.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Btn_Check.Enabled = true;
                });
                Button_Mode.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Button_Mode.Enabled = true;
                });
                Button_Interface.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Button_Interface.Enabled = true;
                });
                textBox_Sensor_Status.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    textBox_Sensor_Status.Text = "Online";
                    textBox_Sensor_Status.ForeColor = Color.Green;
                });
                Sensor_input_Helper.Update_FinishBatch(Sensor_input_Helper.GetLocalIPAddress(), batch_id);

            }
            else
            {
                Sensor_input_Helper.Command_Stop(mySerialPort);
                Thread.Sleep(2000);
                Sensor_input_Helper.Command_Stop(mySerialPort);
                Thread.Sleep(2000);

                Btn_Start.Invoke((Action)delegate
                {
                    Btn_Start.Enabled = false;
                });
                Btn_Stop.Invoke((Action)delegate
                {
                    Btn_Stop.Enabled = false;
                });
                Button_Mode.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    Button_Mode.Enabled = false;
                });
                Btn_CheckTemp.Invoke((Action)delegate
                {
                    Btn_CheckTemp.Enabled = false;
                });
                Btn_Check.Invoke((Action)delegate
                {
                    Btn_Check.Enabled = true;
                });
                textBox_Sensor_Status.Invoke((Action)delegate
                {
                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                    textBox_Sensor_Status.Text = "Error";
                    textBox_Sensor_Status.ForeColor = Color.Red;

                });

                ButtonProduct.Invoke((Action)delegate
                {
                    ButtonProduct.Enabled = false;
                });

                ButtonNumInterval.Invoke((Action)delegate
                {
                    ButtonNumInterval.Enabled = false;
                });

                ButtonNumPcs.Invoke((Action)delegate
                {
                    ButtonNumPcs.Enabled = false;
                });

                ButtonWaitingTime.Invoke((Action)delegate
                {
                    ButtonWaitingTime.Enabled = false;
                });


            }

        }

        private bool check_Error(string check_string)
        {
            bool_check_error = false;
            foreach (string error in List_Error_code)
            {
                if (check_string == error)
                {
                    bool_check_error = true;
                }
            }

            if (bool_check_error)
            {
                batch_id = Sensor_input_Helper.MySql_Insert_Batch(Sensor_input_Helper.GetLocalIPAddress()
            , "0"
            , 0
            , "0"
            , 0
            , "0")
            ;

                Console.WriteLine("Match Error adalah: " + check_string);
                Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id, check_string);
                Error_Sensor_Controller enum_ErrorCode = (Error_Sensor_Controller)Enum.Parse(typeof(Error_Sensor_Controller), "error" + check_string);
                string Error_Message = Sensor_input_Helper.GetDescription(enum_ErrorCode);
                MessageBox.Show(this, Error_Message, application_name);

            }
            return bool_check_error;

        }
        private bool check_Error_during_measurement(string check_string, int batch_id_check)
        {
            bool_check_error = false;
            List<string> error_during_measurement = new List<string>(new string[] { "020", "021", "element3","000" });
            foreach (string error in error_during_measurement)
            {
                if (check_string == error)
                {
                    bool_check_error = true;
                }
            }

            if (bool_check_error)
            {
                Console.WriteLine("Match Error adalah: " + check_string);
                Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id_check, check_string);
                Error_Sensor_Controller enum_ErrorCode = (Error_Sensor_Controller)Enum.Parse(typeof(Error_Sensor_Controller), "error" + check_string);
                string Error_Message = Sensor_input_Helper.GetDescription(enum_ErrorCode);
                MessageBox.Show(this, Error_Message, application_name);


            }
            return bool_check_error;

        }

        #endregion


        #region Timer
        private void MyTimer_CheckStop_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Start MyTimer_CheckStop_Tick");
            TimeSpan Time_dif_check_5min = DateTime.Now - start_5min_check;
            if (Time_dif_check_5min.TotalSeconds > 70) // aslinya 300, sekrang tssting dlu
                                                       //if (Time_dif_check_5min.TotalMinutes > 1) // aslinya 300, sekrang tssting dlu
            {
                MessageBox.Show(this, "Error 011 - no message during checking for 5 mins");

                checkcommand = false;
                Console.WriteLine("Check Thread Aborted");
                Timer_5min_StopCheck.Enabled = false;
                Timer_5min_StopCheck.Stop();
                
            }


        }
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if (blink_timer % 2 == 0)
            {
                Curr_Measure_TextBox.Invoke((Action)delegate
                {
                    Curr_Measure_TextBox.Text = " " + "." + " ";
                });
            }
            else
            {
                Curr_Measure_TextBox.Invoke((Action)delegate
                {
                    Curr_Measure_TextBox.Text = string.Empty;
                   //Curr_Measure_TextBox.Text = " J " + "." + " J ";
                });
            }
            blink_timer = blink_timer + 1;

            //Console.WriteLine("blink_timer adalah: "+ blink_timer.ToString() + " & %2 adalah: " + (blink_timer % 2) .ToString());
            TimeSpan Calibrate = TimeSpan.FromSeconds(20);
            TimeSpan Time_Dif_Timer = DateTime.Now - FixedTime_start;

            if (stat_continue == true
                && Time_Dif_Timer.TotalSeconds - Calibrate.TotalSeconds >= running_time_fixed  // 240 + 100
                && fixed_time_timer_stop == true

                )
            {
                Console.WriteLine("fixed_time_timer_stop adalah: ", fixed_time_timer_stop.ToString());
                fixed_time_timer_stop = false;
                Sensor_input_Helper.Command_Stop(mySerialPort);
                Thread.Sleep(2000);
            }

        }
        #endregion

        #region Thread


        private void CheckTemp_Thread()
        {
            temp_cond = true;
            string Result_Parsing = "";
            while (temp_cond)
            {
                try
                {
                    Thread.Sleep(3000);// this solves the problem
                    byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                    int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                    string readStr = string.Empty;

                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                    readStr = readStr.Trim();
                    Console.WriteLine("ReadStr adalah: " + readStr);

                    string[] charactersToReplace = new string[] { @"r" };
                    foreach (string s in charactersToReplace)
                    {
                        readStr = readStr.Replace(s, "");
                    }

                    char[] delimiter_r = { '\r' };
                    string[] Measures_With_U = readStr.Split(delimiter_r);

                    foreach (string measure in Measures_With_U)
                    {
                        bool isDigitPresent = measure.Any(c => char.IsDigit(c));
                        if (isDigitPresent == true)
                        {
                            Result_Parsing = measure;
                        }

                    }
                    //Result_Parsing = Measures_With_U.Last();
                    int n = 0;
                    bool check_if_number = int.TryParse(Result_Parsing, out n);
                    if (check_Error(Result_Parsing))
                    {
                        next_action_button(true);
                        Console.WriteLine("Error during check Temp");
                        temp_cond = false;
                    }
                    else if (
                        Result_Parsing.Length == 4
                        && Result_Parsing.Any(c => char.IsDigit(c))
                        && check_if_number == true

                        )
                    {
                        Result_Parsing = Result_Parsing.Substring(Result_Parsing.Length - 3);
                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                    , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                        Temp_TextBox.Invoke((Action)delegate
                        {
                            Temp_TextBox.Text = Result_Parsing;
                        });
                        Sensor_input_Helper.Command_Stop(mySerialPort);
                        temp_cond = false;
                    }

                    else
                    {
                        Console.WriteLine("this is checktemp else: " + Result_Parsing);
                    }

                }
                catch (TimeoutException ex)
                {
                    MessageBox.Show(this, "Error 011 - no message during checking for 5 mins");
                    bool error = true;
                    next_action_button(error);

                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();

                    stat_continue = false;
                    start_next_cond = false;
                    aggregate_cond = false;
                    Console.WriteLine(ex.Message);
                }

                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Console.WriteLine(ex);
                    //return "";
                }
            }

            Console.WriteLine("Finsih_Check_Temp");
            //return Result_Parsing;
            //Sensor_input_Helper.Command_Stop(mySerialPort);
        }
        private void Check_Thread()
        {
            try
            {

                Sensor_input_Helper.Command_Check(mySerialPort);
                Thread.Sleep(12000);

                checkcommand = true;
                string Result_Parsing = "";
                bool check_db = check_db_connection();

                while (checkcommand && check_db)
                {
                    //start_5min_check = DateTime.Now;

                    //Timer_5min_StopCheck.Enabled = true;
                    //Timer_5min_StopCheck.Start();

                    Sensor_input_Helper.Command_CheckData(mySerialPort);
                    Thread.Sleep(2000);// this solves the problem
                    string readStr = string.Empty;
                    byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                    int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                    readStr = readStr.Trim();

                    Console.WriteLine("ReadStr adalah: " + readStr);

                    string[] charactersToReplace = new string[] { @"r" };
                    foreach (string s in charactersToReplace)
                    {
                        readStr = readStr.Replace(s, "");
                    }

                    char[] delimiter_r = { '\r' };
                    string[] Measures_With_U = readStr.Split(delimiter_r);

                    foreach (string measure in Measures_With_U)
                    {
                        bool isDigitPresent = measure.Any(c => char.IsDigit(c));
                        if (isDigitPresent == true)
                        {
                            Result_Parsing = measure;
                        }

                    }
                    //if (check_Error(Result_Parsing) || check_5min_error(start_5min_check))
                    if (check_Error(Result_Parsing))
                    {
                        next_action_button(true);
                        checkcommand = false;
                    }
                    else if (Result_Parsing == "00090")
                    {
                        Thread.Sleep(8000);
                        Console.WriteLine("Sensor Normal");
                        //MessageBox.Show(this, "Connection Succeed");
                        checkcommand = false;
                        data_cleansing();
                        next_action_button(false);

                    }
                    else
                    {
                        Console.WriteLine("Check not found. This is result parsing : " + Result_Parsing);
                    }
                    //Timer_5min_StopCheck.Enabled = false;
                    //Timer_5min_StopCheck.Stop();
                }
            }
            catch (TimeoutException ex)
            {
                next_action_button(true);

                Thread.Sleep(10000);

                MessageBox.Show(this, "Error 011 - No message return during checking");

                checkcommand = false;
                //Console.WriteLine("Check Thread Aborted");
                Console.WriteLine(ex.Message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void Read_Interval_Thread()
        {

            string forever_str;
            string readStr;
            bool Measure_Cond = true;

            byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
            int readLen;
            string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };
            string Result_Parsing;
            bool countingbatch;
            const char STX = '\u0002';
            const char ETX = '\u0003';
            List<string> AllText = new List<string>();
            Data_Measure_Result = new List<data_measure_2> { };
            counter_data = 0;
            //Data_Measure_Result
            DateTime date_start_ReadInterval = DateTime.Now;

            while (stat_continue)
            {
                try
                {
                    readStr = string.Empty;
                    forever_str = string.Empty;
                    aggregate_cond = true;
                    start_next_cond = true;
                    Measure_Cond = true;
                    countingbatch = true;
                    Thread.Sleep(3000);
                    MyTimer.Enabled = true;
                    //MyTimer.Start();
                    Console.WriteLine("Start Timer");
                    #region Collect Measurement Value

                    while (Measure_Cond == true)
                    {
                        Thread.Sleep(1000);// this solves the problem
                        readBuffer = new byte[mySerialPort.ReadBufferSize];
                        readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                        //string readStr = string.Empty;
                        Console.WriteLine("ReadStr original adalah: " + Encoding.UTF8.GetString(readBuffer, 0, readLen));
                        forever_str = forever_str + Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        // Data Cleansing

                        if (readStr != "" && readStr != null)
                        {
                            char[] delimiter_r = { '\r' };

                            if (readStr.Any(c => char.IsDigit(c)) && !readStr.Trim().ToLower().Contains("r"))
                            {
                                readStr = readStr.Trim();
                                Console.WriteLine("ReadStr Trim adalah: " + readStr);
                                string[] Measures_With_U = readStr.Split(delimiter_r); // misahin antar nilai
                                List<string> Measure_Results = new List<string>();

                                foreach (var Measure in Measures_With_U)
                                {

                                    Result_Parsing = GetWords(Measure).FirstOrDefault(); // hilangin ETX dan STX
                                    if (Result_Parsing != "" && Result_Parsing != null)
                                    {
                                        foreach (string s in charactersToReplace)
                                        {
                                            Result_Parsing = Result_Parsing.Replace(s, "");
                                        }
                                    }

                                    if (Result_Parsing != "" && Result_Parsing != null && !Result_Parsing.Trim().ToLower().Contains("r"))
                                    {
                                        // check error
                                        Console.WriteLine("Result_Parsing & Batch_ID adalah: " + Result_Parsing + " " + batch_id.ToString());
                                        if (check_Error_during_measurement(Result_Parsing, batch_id))
                                        {
                                            aggregate_cond = false;
                                            Measure_Cond = false;
                                            countingbatch = false;
                                            bool_check_error = true;
                                            Console.WriteLine("MyTimerStop");
                                        }
                                        // Finsih check error

                                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                            , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                        Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString("0.0");

                                        counter_data_reset = counter_data_reset + 1;
                                        Console.WriteLine("nilai measure adalah: " + Result_Parsing); // ganti jadi
                                        Curr_Kernel_TextBox.Invoke((Action)delegate
                                        {
                                            //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                            Curr_Kernel_TextBox.Text = (counter_data_reset).ToString();
                                        });

                                        
                                        if (thereshold_param == true && (double.Parse(Result_Parsing) > therehold_max || double.Parse(Result_Parsing) < thereshold_min))
                                        {
                                            Sensor_input_Helper.Callbeep();
                                        }


                                        #region thereshold temporary
                                        if (thereshold_param == true)
                                        {
                                            if (double.Parse(Result_Parsing) > therehold_max)
                                            {
                                                thereshold_max_counter = thereshold_max_counter + 1;
                                                
                                                textBox_theresholdmax.Invoke((Action)delegate
                                                {
                                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                                    textBox_theresholdmax.Text = (thereshold_max_counter).ToString();
                                                });
                                                

                                            }
                                            
                                            if(double.Parse(Result_Parsing) < thereshold_min)
                                            {
                                                thereshold_min_counter = thereshold_min_counter + 1;

                                                textBox_theresholdmin.Invoke((Action)delegate
                                                {
                                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                                    textBox_theresholdmin.Text = (thereshold_min_counter).ToString();
                                                });
                                                
                                            }

                                        }
                                        #endregion


                                        //float Result_Parsing_input = float.Parse(Result_Parsing);
                                        readStr = string.Empty;
                                    }
                                }
                                // klo ada measurement. mulai olah
                                // masukin olah data yag lama 
                            }

                            else if 
                                (
                                    (
                                        readStr.Trim().ToLower().Contains("r")
                                        //&& counter_data_reset > (int.Parse(ButtonNumPcs.Text) / 2)
                                        && counter_data_reset >= 1
                                        && !readStr.Any(c => char.IsDigit(c))
                                        && countingbatch == true
                                    )
                                        || bool_stop_click == true
                                 )
                            {
                                //counter_data = 0;
                                counter_data_reset = 0;
                                Console.WriteLine("Forever_str original adalah: " + forever_str);

                                string[] Measures_With_U = forever_str.Split(delimiter_r); // misahin antar nilai

                                foreach (var Measure in Measures_With_U)
                                {
                                    bool test1 = Measure.Any(c => char.IsDigit(c));
                                    bool test2 = !Measure.Trim().ToLower().Contains("r");
                                    bool test3 = Measure.Contains(STX);
                                    bool test4 = Measure.Contains(ETX);

                                    //bool test3 = Measure.Trim().ToLower().Contains("u0002");
                                    //bool test4 = Measure.Trim().ToLower().Contains("u0003");

                                    if (test1 && test2 && test3 && test4)
                                    {
                                        Result_Parsing = GetWords(Measure).FirstOrDefault(); // hilangin ETX dan STX
                                        foreach (string s in charactersToReplace)
                                        {
                                            Result_Parsing = Result_Parsing.Replace(s, "");
                                        }

                                        
                                        #region compare checksum

                                        string checksum_parsing = Measure.Substring(5, 2);

                                        bool checksum_result = Sensor_input_Helper.checksum(Result_Parsing, checksum_parsing);
                                        Console.WriteLine("Test: ");
                                        Console.WriteLine("result_parsing adalah: " + Result_Parsing);
                                        Console.WriteLine("checksum_parsing adalah: " + checksum_parsing);
                                        Console.WriteLine("checksum_result adalah: " + checksum_result);

                                        if (!checksum_result)
                                        {
                                            bool_checksum_error = true;
                                        }

                                        // 2-4 nilai 
                                        // 6-7 checksum


                                        #endregion
                                        // Data cleansing
                                        

                                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                                , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                                        Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString();

                                        counter_data = Data_Measure_Result.Count;

                                        Data_Measure_Current = new data_measure_2(counter_data + 1
                                            , Result_Parsing
                                            , (DateTime.Now).ToString());
                                        Data_Measure_Result.Add(Data_Measure_Current);

                                        Console.WriteLine("nilai measure forever str parsing result adalah: " + Result_Parsing); // ganti jadi

                                        float Result_Parsing_input = float.Parse(Result_Parsing);
                                        Sensor_input_Helper.MySql_Insert_Measure(batch_id, counter_data + 1, Result_Parsing_input
                                            , DateTime.Now, 0, current_interval + 1);
                                        counter_data_reset = counter_data_reset + 1;
                                        //readStr = string.Empty;

                                        // Thereshold Max
                                        //int count = Data_Measure_Result.Count(x => x.Measures < 5);

                                        #region theresholdfinal
                                        int theresholdmax_counter = Data_Measure_Result.Count(x => float.Parse( x.Measures) > therehold_max);

                                        textBox_theresholdmax.Invoke((Action)delegate
                                        {
                                            //ListFilesToProcess.Count(item => item.IsChecked);
                                            //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                            textBox_theresholdmax.Text = theresholdmax_counter.ToString();
                                        });

                                        int theresholdmin_counter = Data_Measure_Result.Count(x => float.Parse(x.Measures) < thereshold_min);

                                        textBox_theresholdmin.Invoke((Action)delegate
                                        {
                                            //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                            textBox_theresholdmin.Text = (thereshold_min_counter).ToString();
                                        });

                                        // Thereshold min
                                        #endregion


                                    }
                                    else
                                    {
                                        Console.WriteLine("R");
                                    }


                                }

                                // Checksum test
                                if (bool_checksum_error == true)
                                {
                                    aggregate_cond = false;
                                    Measure_Cond = false;
                                    countingbatch = false;
                                    bool_check_error = true;
                                    Console.WriteLine("MyTimerStop");

                                    Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id, "030");
                                    MessageBox.Show(this, "Error-030, Error during checksum ", application_name);


                                }

                                Curr_Kernel_TextBox.Invoke((Action)delegate
                                {
                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                    Curr_Kernel_TextBox.Text = counter_data_reset.ToString();
                                });
                                Measure_Cond = false;
                                countingbatch = false;
                            }

                            else
                            {
                                Console.WriteLine("Nilainya Readstr Dari if else null adalah: " + readStr);
                            }

                        }

                        else
                        {
                            Console.WriteLine("Nilainya Readstr null adalah: " + readStr);
                        }
                        //string input = "hello123world";
                        //bool isDigitPresent = input.Any(c => char.IsDigit(c));

                    }

                    #endregion

                    #region Get Aggregate value

                    //start_next_init = 0;
                    //OpenCon_Port_local(mySerialPort, BaudRate);
                    while (aggregate_cond)
                    {
                        Result_Parsing = string.Empty;
                        Console.WriteLine("Start Aggregate_cond");
                        Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);
                        Thread.Sleep(2000);// this solves the problem
                        readBuffer = new byte[mySerialPort.ReadBufferSize];
                        readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                        readStr = string.Empty;
                        readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        readStr = readStr.Trim();

                        Console.WriteLine("ReadStr Average adalah: " + readStr);
                        foreach (string s in charactersToReplace)
                        {
                            Result_Parsing = readStr.Replace(s, "");
                        }

                        //Result_Parsing = GetWords(Result_Parsing).FirstOrDefault();
                        if (Result_Parsing != null)
                        {
                            if (
                                Result_Parsing.Contains("-")
                                && (Result_Parsing.Length) > 4
                                && Result_Parsing.Contains(STX)
                                && Result_Parsing.Contains(ETX)
                                )
                            {
                                MyTimer.Enabled = false;
                                //MyTimer.Stop();

                                AllText = GetWords(Result_Parsing);
                                int checkindex;
                                string aggregate_value_string = string.Empty;
                                foreach (string text in AllText)
                                {
                                    if (
                                        text.Length >= 10
                                        //&& text.Length <= 12
                                        && !text.Trim().ToLower().ToString().Contains("r")
                                        )
                                    {
                                        aggregate_value_string = text;
                                    }
                                }

                                Result_Parsing = aggregate_value_string.Substring(5, 3);
                                Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                    , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString("0.0");

                                Data_Avg_Result.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                aggregate_cond = false;


                                Curr_Measure_TextBox.Invoke((Action)delegate
                                {
                                    //Curr_Measure_TextBox.Text = Result_Parsing.Format("0.0");
                                    Curr_Measure_TextBox.Text = string.Format("{0:F1}", Result_Parsing) + "%";
                                    
                                });

                                total_average = 0;
                                Current_Avg_TextBox.Invoke((Action)delegate
                                {
                                    foreach (data_measure_2 average_val in Data_Avg_Result)
                                    {
                                        total_average = total_average + float.Parse(average_val.Measures);
                                    }

                                    total_current_Average = total_average / Data_Avg_Result.Count();
                                    Current_Avg_TextBox.Text = total_current_Average.ToString("0.00") + "%";
                                    //Final Average
                                });


                                //loat Result_Parsing_input = float.Parse(Result_Parsing);
                                Sensor_input_Helper.MySql_Insert_Measure(batch_id, 1000 + current_interval + 1
                                    , float.Parse(Result_Parsing), DateTime.Now, 1, current_interval + 1);

                                Console.WriteLine("Finish Aggregate");
                                readStr = string.Empty;
                            }
                            else if (
                                 (Data_Measure_Result.Count == 0 && Result_Parsing.Substring(3, 5) == "00000")
                                 || (!Result_Parsing.Contains("-") && (Result_Parsing.Length) > 10)
                                 )
                            {
                                Result_Parsing = "0.0";

                                Data_Avg_Result.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                aggregate_cond = false;


                                Curr_Measure_TextBox.Invoke((Action)delegate
                                {
                                    Curr_Measure_TextBox.Text = Result_Parsing + "%";
                                });

                                Current_Avg_TextBox.Invoke((Action)delegate
                                {
                                    foreach (data_measure_2 average_val in Data_Avg_Result)
                                    {
                                        total_average = total_average + float.Parse(average_val.Measures);
                                    }

                                    total_current_Average = total_average / Data_Avg_Result.Count();
                                    Current_Avg_TextBox.Text = total_current_Average.ToString("0.0") + "%";
                                    //Final Average
                                });

                            }
                            else
                            {
                                Console.WriteLine("Aggreagte empty");
                            }

                        }
                        //start_next_init++;
                    }

                    #endregion Finish get aggregate value
                    Console.WriteLine("Finish aggregate region");

                    #region Finish All Measure and close port

                    Console.WriteLine("data_average count adalah: ", Data_Avg_Result.Count().ToString());
                    //Console.WriteLine("data_average count adalah: ", current_interval.ToString());

                    if (Data_Avg_Result.Count() == TotalInterval || bool_check_error == true || bool_stop_click == true)
                    {
                        Console.WriteLine("End All Measurement ");
                        stat_continue = false;
                        mySerialPort.DiscardInBuffer();
                        mySerialPort.DiscardOutBuffer();

                        stat_continue = false;
                        start_next_cond = false;
                        aggregate_cond = false;

                        finish_measurement = 1;

                        next_action_button(bool_check_error);

                    }


                    #endregion

                    #region delay start
                    if (start_next_cond == true)
                    {
                        #region Delay start
                        Console.WriteLine("start delay", "start delay");
                        //mySerialPort.Close();
                        Thread.Sleep(delay);
                        Console.WriteLine("Finish delay", "Finish delay");
                        #endregion
                    }
                    #endregion


                    #region Start Next sequence

                    while (start_next_cond)
                    {
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
                        Thread.Sleep(1000);
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                        current_interval++;
                        Curr_Interval_TextBox.Invoke((Action)delegate
                        {
                            Curr_Interval_TextBox.Text = (current_interval + 1).ToString();
                        });

                        start_next_cond = false;
                        blink_timer = 1;
                        counter_data_reset = 0;
                        readStr = string.Empty;


                    }
                    #endregion

                }
                catch (TimeoutException ex)
                {

                    bool error = true;
                    next_action_button(error);
                    Thread.Sleep(10000);
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();

                    stat_continue = false;
                    start_next_cond = false;
                    aggregate_cond = false;
                    Console.WriteLine(ex.Message);
                    Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id, "011");
                    MessageBox.Show(this, "Error 011 - There is no grain feed to sensor for more than five minutes.");
                }

                catch (Exception ex)
                {
                    //Trace.TraceError(ex.Message);
                    Console.WriteLine(ex.Message);
                    //return "";
                }

            }

            //MessageBox.Show("measurement finsih");
            Console.WriteLine("Measurement Finish");
        }
        public void Read_FixedTime_Thread()
        {

            string forever_str = string.Empty; ;
            string readStr;
            bool Measure_Cond = true;
            byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
            int readLen;
            string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };
            string Result_Parsing;
            bool countingbatch;
            const char STX = '\u0002';
            const char ETX = '\u0003';
            List<string> AllText = new List<string>();
            Data_Measure_Result = new List<data_measure_2> { };
            counter_data = 0;
            //Data_Measure_Result

            //DateTime date_start_ReadFixedTime = DateTime.Now;
            char[] delimiter_r = { '\r' };

            Console.WriteLine("Running Time Fixed is: " + running_time_fixed.ToString());
            while (stat_continue)
            {

                try
                {

                    readStr = string.Empty;
                    aggregate_cond = true;
                    start_next_cond = true;
                    Measure_Cond = true;
                    countingbatch = true;
                    Thread.Sleep(3000);
                    MyTimer.Enabled = true;
                    //MyTimer.Start();

                    #region Collect Measurement Value

                    while (Measure_Cond == true)
                    {

                        Thread.Sleep(1000);// this solves the problem
                        readBuffer = new byte[mySerialPort.ReadBufferSize];
                        readLen = -1;
                        readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                        Console.WriteLine("ReadStr original adalah: " + Encoding.UTF8.GetString(readBuffer, 0, readLen));
                        forever_str = forever_str + Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);

                        #region Add Interval
                        if (readStr != "" && readStr != null)
                        {
                            if (readStr.Any(c => char.IsDigit(c)) && !readStr.Trim().ToLower().Contains("r"))
                            {
                                readStr = readStr.Trim();
                                Console.WriteLine("ReadStr Trim adalah: " + readStr);
                                string[] Measures_With_U = readStr.Split(delimiter_r); // misahin antar nilai
                                List<string> Measure_Results = new List<string>();

                                foreach (var Measure in Measures_With_U)
                                {

                                    Result_Parsing = GetWords(Measure).FirstOrDefault(); // hilangin ETX dan STX
                                    if (Result_Parsing != "" && Result_Parsing != null)
                                    {
                                        foreach (string s in charactersToReplace)
                                        {
                                            Result_Parsing = Result_Parsing.Replace(s, "");
                                        }
                                    }

                                    if (Result_Parsing != "" && Result_Parsing != null && !Result_Parsing.Trim().ToLower().Contains("r"))
                                    {
                                        // check error
                                        Console.WriteLine("Measure & Batch_ID adalah: " + Result_Parsing + " " + batch_id.ToString());
                                        if (check_Error_during_measurement(Result_Parsing, batch_id))
                                        {
                                            aggregate_cond = false;
                                            Measure_Cond = false;
                                            countingbatch = false;
                                            bool_check_error = true;
                                            MyTimer.Enabled = false;
                                            //MyTimer.Stop();
                                            Console.WriteLine("Timer Stop");
                                        }
                                        // FInsih check error


                                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                            , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                        counter_data_reset = counter_data_reset + 1;
                                        Console.WriteLine("nilai measure adalah: " + Result_Parsing); // ganti jadi
                                        Curr_Kernel_TextBox.Invoke((Action)delegate
                                        {
                                            //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                            Curr_Kernel_TextBox.Text = (counter_data_reset).ToString();
                                        });

                                        /*
                                        if (thereshold_param == true && (double.Parse(Result_Parsing) > therehold_max || double.Parse(Result_Parsing) < thereshold_min))
                                        {

                                            Sensor_input_Helper.Callbeep();
                                        }
                                        */

                                        #region thereshold temporary


                                        if (thereshold_param == true)
                                        {
                                            if (double.Parse(Result_Parsing) > therehold_max)
                                            {
                                                thereshold_max_counter = thereshold_max_counter + 1;

                                                textBox_theresholdmax.Invoke((Action)delegate
                                                {
                                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                                    textBox_theresholdmax.Text = (thereshold_max_counter).ToString();
                                                });


                                            }
                                            if (double.Parse(Result_Parsing) < thereshold_min)
                                            {
                                                thereshold_min_counter = thereshold_min_counter + 1;

                                                textBox_theresholdmin.Invoke((Action)delegate
                                                {
                                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                                    textBox_theresholdmin.Text = (thereshold_min_counter).ToString();
                                                });

                                            }
                                            else
                                            {
                                                Console.WriteLine("did not reach thershold");
                                            }

                                        }
                                        #endregion


                                        //float Result_Parsing_input = float.Parse(Result_Parsing);
                                        readStr = string.Empty;
                                    }
                                }
                                // klo ada measurement. mulai olah
                                // masukin olah data yag lama 
                            }

                            else if ( // start new batch
                                readStr.Trim().ToLower().Contains("r")
                                && counter_data_reset > 1
                                && !readStr.Any(c => char.IsDigit(c))
                                && countingbatch == true
                                && bool_stop_click == false
                                )
                            {
                                Sensor_input_Helper.Command_Write(mySerialPort, "12598\r"); // max value

                                Thread.Sleep(1000);
                                Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                                Console.WriteLine("Next measurement Fixed Time");
                                //start_next_cond = false;
                                blink_timer = 1;
                                //counter_data_reset = 0;
                                readStr = string.Empty;
                            }
                            else
                            {
                                Console.WriteLine("Nilainya Readstr Dari if else null adalah: " + readStr);
                            }

                        }

                        else
                        {
                            Console.WriteLine("Nilainya Readstr null adalah: " + readStr);
                        }

                        #endregion


                        #region finish measurement
                        Time_Dif = DateTime.Now - FixedTime_start;
                        if (Time_Dif.TotalSeconds > running_time_fixed || bool_check_error == true || bool_stop_click == true) // change from time.in seconds
                        {
                            Console.WriteLine("DateTime Now & FixedTime_Start: " + DateTime.Now.ToString() + " & " + FixedTime_start.ToString());
                            Console.WriteLine("Time Dif Total second & Running Time Fixed adalah: " + Time_Dif.TotalSeconds.ToString() + " & " + running_time_fixed.ToString());
                            MyTimer.Enabled = false;
                            //MyTimer.Stop();

                            if (!bool_stop_click)
                            {
                                Sensor_input_Helper.Command_Stop(mySerialPort);
                                Thread.Sleep(3000);
                                Sensor_input_Helper.Command_Stop(mySerialPort);
                                Thread.Sleep(3000);
                                Console.WriteLine("Send Stop for fixed Time");
                            }
                            else
                            {
                                Console.WriteLine("Dont Send Stop for fixed Time");

                            }
                            //Sensor_input_Helper.Command_Stop(mySerialPort);
                            //Thread.Sleep(3000);

                            string[] Measures_With_U = forever_str.Split(delimiter_r); // misahin antar nilai
                            counter_data_reset = 0;
                            foreach (var Measure in Measures_With_U)
                            {
                                bool test1 = Measure.Any(c => char.IsDigit(c));
                                bool test2 = !Measure.Trim().ToLower().Contains("r");
                                bool test3 = Measure.Contains(STX);
                                bool test4 = Measure.Contains(ETX);
                                if (test1 && test2 && test3 && test4)
                                {
                                    Result_Parsing = GetWords(Measure).FirstOrDefault(); // hilangin ETX dan STX
                                                                                         // Data cleansing
                                    foreach (string s in charactersToReplace)
                                    {
                                        Result_Parsing = Result_Parsing.Replace(s, "");
                                    }
                                    Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                            , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                                    Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString("0.0");

                                    counter_data = Data_Measure_Result.Count;

                                    Data_Measure_Current = new data_measure_2(counter_data + 1
                                        , Result_Parsing
                                        , (DateTime.Now).ToString());
                                    Data_Measure_Result.Add(Data_Measure_Current);

                                    Console.WriteLine("nilai measure forever str parsing result adalah: " + Result_Parsing); // ganti jadi

                                    float Result_Parsing_input = float.Parse(Result_Parsing);
                                    Sensor_input_Helper.MySql_Insert_Measure(batch_id, counter_data + 1, Result_Parsing_input, DateTime.Now, 0, current_interval + 1);
                                    //counter_data_reset = counter_data_reset + 1;
                                    //readStr = string.Empty;
                                    Curr_Kernel_TextBox.Invoke((Action)delegate
                                    {
                                        //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                        Curr_Kernel_TextBox.Text = Data_Measure_Result.Count().ToString();
                                    });


                                    #region theresholdfinal
                                    int theresholdmax_counter = Data_Measure_Result.Count(x => float.Parse(x.Measures) > therehold_max);

                                    textBox_theresholdmax.Invoke((Action)delegate
                                    {
                                        //ListFilesToProcess.Count(item => item.IsChecked);
                                        //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                        textBox_theresholdmax.Text = theresholdmax_counter.ToString();
                                    });

                                    int theresholdmin_counter = Data_Measure_Result.Count(x => float.Parse(x.Measures) < thereshold_min);

                                    textBox_theresholdmin.Invoke((Action)delegate
                                    {
                                        //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                        textBox_theresholdmin.Text = (thereshold_min_counter).ToString();
                                    });

                                    // Thereshold min
                                    #endregion



                                }
                                else
                                {
                                    Console.WriteLine("R-nya isinya trash");
                                }
                            }

                            #region Get Aggregate value
                            while (aggregate_cond)
                            {

                                Console.WriteLine("Start Aggregate_cond");
                                Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);
                                Thread.Sleep(4000);// this solves the problem
                                Result_Parsing = string.Empty;
                                readBuffer = new byte[mySerialPort.ReadBufferSize];
                                readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                                readStr = string.Empty;
                                readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                                readStr = readStr.Trim();

                                Console.WriteLine("ReadStr Average adalah: " + readStr);

                                foreach (string s in charactersToReplace)
                                {
                                    Result_Parsing = readStr.Replace(s, "");
                                }

                                if (Result_Parsing != null)
                                {
                                    Console.WriteLine("Result parsing adalah: " + Result_Parsing);
                                    if (
                                        Result_Parsing.Contains(STX)
                                        && Result_Parsing.Contains(ETX)
                                        && Result_Parsing.Contains("-")
                                        && (Result_Parsing.Length) > 8
                                        && Data_Measure_Result.Count >= 1
                                        )
                                    {


                                        AllText = GetWords(Result_Parsing);
                                        string aggregate_value_string = string.Empty;
                                        foreach (string text in AllText)
                                        {
                                            if (
                                                text.Length >= 10
                                                && !text.Trim().ToLower().ToString().Contains("r")
                                                )
                                            {
                                                aggregate_value_string = text;
                                            }
                                        }
                                        Console.WriteLine("ReadStr Aggreaget_value_string adalah: " + aggregate_value_string);

                                        Result_Parsing = aggregate_value_string.Substring(5, 3);
                                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                            , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                        Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString();

                                        Data_Avg_Result.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                        aggregate_cond = false;


                                        Curr_Measure_TextBox.Invoke((Action)delegate
                                        {
                                            //Curr_Measure_TextBox.Text = Result_Parsing.Format("0.0");
                                            Curr_Measure_TextBox.Text = string.Format("{0:F1}", Result_Parsing) + "%";
                                        });

                                        total_average = 0;
                                        Current_Avg_TextBox.Invoke((Action)delegate
                                        {
                                            foreach (data_measure_2 average_val in Data_Avg_Result)
                                            {
                                                total_average = total_average + float.Parse(average_val.Measures);
                                            }

                                            total_current_Average = total_average / Data_Avg_Result.Count();
                                            Current_Avg_TextBox.Text = total_current_Average.ToString("0.00") + "%";
                                            //Final Average
                                        });

                                        //loat Result_Parsing_input = float.Parse(Result_Parsing);
                                        Sensor_input_Helper.MySql_Insert_Measure(batch_id, 1000 + current_interval + 1, float.Parse(Result_Parsing), DateTime.Now, 1, current_interval + 1);

                                        Console.WriteLine("Finish Aggregate");
                                        readStr = string.Empty;

                                    }

                                    else if (
                                        (Data_Measure_Result.Count == 0 && Result_Parsing.Substring(3,5) == "00000") 
                                        || (!Result_Parsing.Contains("-") && (Result_Parsing.Length) > 10)

                                        )
                                    {
                                        Result_Parsing = "0.0";

                                        Data_Avg_Result.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                        aggregate_cond = false;


                                        Curr_Measure_TextBox.Invoke((Action)delegate
                                        {
                                            Curr_Measure_TextBox.Text = Result_Parsing + "%";
                                        });

                                        Current_Avg_TextBox.Invoke((Action)delegate
                                        {
                                            foreach (data_measure_2 average_val in Data_Avg_Result)
                                            {
                                                total_average = total_average + float.Parse(average_val.Measures);
                                            }

                                            total_current_Average = total_average / Data_Avg_Result.Count();
                                            Current_Avg_TextBox.Text = total_current_Average.ToString("0.0") + "%";
                                            //Final Average
                                        });

                                    }
                                    else
                                    {
                                        Console.WriteLine("Aggreagte empty");
                                    }

                                }
                                //start_next_init++;
                            }

                            #endregion Finish get aggregate value

                            // Stop measurement
                            stat_continue = false;
                            start_next_cond = false;
                            aggregate_cond = false;
                            Measure_Cond = false;

                            next_action_button(bool_check_error);

                        }

                        #endregion

                    }

                    #endregion


                    #region Start Next sequence

                    while (start_next_cond)
                    {
                        //Sensor_input_Helper.Command_Write(mySerialPort, "12598\r"); // max value
                        Sensor_input_Helper.Command_Write(mySerialPort, "10192\r"); // max value
                        Thread.Sleep(1000);
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                        current_interval++;

                        blink_timer = 1;
                        readStr = string.Empty;
                    }
                    #endregion


                }
                catch (TimeoutException ex)
                {


                    MyTimer.Enabled = false;
                    //MyTimer.Stop();
                    //mySerialPort.DiscardInBuffer();
                    //mySerialPort.DiscardOutBuffer();
                    stat_continue = false;
                    start_next_cond = false;
                    aggregate_cond = false;
                    Measure_Cond = false;
                    bool error = true;
                    next_action_button(error);
                    Thread.Sleep(10000);
                    Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id, "011");
                    MessageBox.Show(this, "Error 011 - no message during checking for 5 mins");
                    Console.WriteLine(ex.Message);

                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Console.WriteLine(ex);
                    //return "";
                }

            }

            //MessageBox.Show("measurement finsih");
            Console.WriteLine("Measurement Finish");
        }
        public void Read_FixedPieces_Thread()
        {

            string forever_str;
            string readStr;
            bool Measure_Cond = true;

            byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
            int readLen;
            string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };
            string Result_Parsing;
            bool countingbatch;
            const char STX = '\u0002';
            const char ETX = '\u0003';
            List<string> AllText = new List<string>();
            Data_Measure_Result = new List<data_measure_2> { };
            counter_data = 0;
            DateTime Date_Start_5min_FixedPieces = DateTime.Now;
            //Data_Measure_Result

            while (stat_continue)
            {
                try
                {
                    readStr = string.Empty;
                    forever_str = string.Empty;
                    aggregate_cond = true;
                    start_next_cond = true;
                    Measure_Cond = true;
                    countingbatch = true;
                    Thread.Sleep(3000);
                    MyTimer.Enabled = true;
                    //MyTimer.Start();
                    Console.WriteLine("MyTimer Start");

                    #region Collect Measurement Value

                    while (Measure_Cond == true)
                    {
                        Thread.Sleep(1500);// this solves the problem
                        readBuffer = new byte[mySerialPort.ReadBufferSize];
                        readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                        //string readStr = string.Empty;
                        Console.WriteLine("ReadStr original adalah: " + Encoding.UTF8.GetString(readBuffer, 0, readLen));
                        forever_str = forever_str + Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        // Data Cleansing

                        if (readStr != "" && readStr != null)
                        {
                            char[] delimiter_r = { '\r' };

                            if (readStr.Any(c => char.IsDigit(c)) && !readStr.Trim().ToLower().Contains("r"))
                            {
                                readStr = readStr.Trim();
                                Console.WriteLine("ReadStr Trim adalah: " + readStr);
                                string[] Measures_With_U = readStr.Split(delimiter_r); // misahin antar nilai
                                List<string> Measure_Results = new List<string>();

                                foreach (var Measure in Measures_With_U)
                                {
                                    Result_Parsing = GetWords(Measure).FirstOrDefault(); // hilangin ETX dan STX
                                    if (Result_Parsing != "" && Result_Parsing != null)
                                    {
                                        foreach (string s in charactersToReplace)
                                        {
                                            Result_Parsing = Result_Parsing.Replace(s, "");
                                        }
                                    }

                                    if (Result_Parsing != "" && Result_Parsing != null && !Result_Parsing.Trim().ToLower().Contains("r"))
                                    {
                                        // check error
                                        Console.WriteLine("Result_Parsing & Batch_ID adalah: " + Result_Parsing + " " + batch_id.ToString());
                                        if (check_Error_during_measurement(Result_Parsing, batch_id))
                                        {
                                            aggregate_cond = false;
                                            Measure_Cond = false;
                                            countingbatch = false;
                                            bool_check_error = true;

                                        }
                                        // FInsih check error

                                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                            , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                        counter_data_reset = counter_data_reset + 1;
                                        Console.WriteLine("nilai measure adalah: " + Result_Parsing); // ganti jadi
                                        Curr_Kernel_TextBox.Invoke((Action)delegate
                                        {
                                            //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                            Curr_Kernel_TextBox.Text = (counter_data_reset).ToString();
                                        });
                                        if (thereshold_param == true && (double.Parse(Result_Parsing) > therehold_max || double.Parse(Result_Parsing) < thereshold_min))
                                        {
                                            Sensor_input_Helper.Callbeep();
                                        }


                                        #region thereshold temporary


                                        if (thereshold_param == true)
                                        {
                                            if (double.Parse(Result_Parsing) > therehold_max)
                                            {
                                                thereshold_max_counter = thereshold_max_counter + 1;

                                                textBox_theresholdmax.Invoke((Action)delegate
                                                {
                                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                                    textBox_theresholdmax.Text = (thereshold_max_counter).ToString();
                                                });


                                            }
                                            if (double.Parse(Result_Parsing) < thereshold_min)
                                            {
                                                thereshold_min_counter = thereshold_min_counter + 1;

                                                textBox_theresholdmin.Invoke((Action)delegate
                                                {
                                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                                    textBox_theresholdmin.Text = (thereshold_min_counter).ToString();
                                                });

                                            }
                                            else
                                            {
                                                Console.WriteLine("did not reach thershold");
                                            }

                                        }
                                        #endregion

                                        //float Result_Parsing_input = float.Parse(Result_Parsing);
                                        readStr = string.Empty;
                                    }
                                }
                                // klo ada measurement. mulai olah
                                // masukin olah data yag lama 
                            }

                            else if (
                                (
                                readStr.Trim().ToLower().Contains("r")
                                //&& counter_data_reset > (int.Parse(ButtonNumPcs.Text) / 2)
                                && counter_data_reset >= 1

                                && !readStr.Any(c => char.IsDigit(c))
                                && countingbatch == true
                                )
                                || bool_stop_click == true

                                )
                            {
                                //counter_data = 0;
                                counter_data_reset = 0;
                                Console.WriteLine("Forever_str original adalah: " + forever_str);

                                string[] Measures_With_U = forever_str.Split(delimiter_r); // misahin antar nilai

                                foreach (var Measure in Measures_With_U)
                                {
                                    bool test1 = Measure.Any(c => char.IsDigit(c));
                                    bool test2 = !Measure.Trim().ToLower().Contains("r");
                                    bool test3 = Measure.Contains(STX);
                                    bool test4 = Measure.Contains(ETX);

                                    //bool test3 = Measure.Trim().ToLower().Contains("u0002");
                                    //bool test4 = Measure.Trim().ToLower().Contains("u0003");

                                    if (test1 && test2 && test3 && test4)
                                    {
                                        string checksum = (Measure.Substring(5, 2));
                                        Console.WriteLine("nilai checksum adalah: ", checksum);


                                        Result_Parsing = GetWords(Measure).FirstOrDefault(); // hilangin ETX dan STX
                                                                                             // Data cleansing
                                        foreach (string s in charactersToReplace)
                                        {
                                            Result_Parsing = Result_Parsing.Replace(s, "");
                                        }
                                        Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                                , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                                        Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString();

                                        counter_data = Data_Measure_Result.Count;

                                        Data_Measure_Current = new data_measure_2(counter_data + 1
                                            , Result_Parsing
                                            , (DateTime.Now).ToString());
                                        Data_Measure_Result.Add(Data_Measure_Current);

                                        Console.WriteLine("nilai measure forever str parsing result adalah: " + Result_Parsing); // ganti jadi

                                        float Result_Parsing_input = float.Parse(Result_Parsing);
                                        Sensor_input_Helper.MySql_Insert_Measure(batch_id, counter_data + 1, Result_Parsing_input, DateTime.Now, 0, 1);
                                        counter_data_reset = counter_data_reset + 1;
                                        //readStr = string.Empty;

                                    }
                                    else
                                    {
                                        Console.WriteLine("R-nya isinya trash");
                                    }


                                }
                                // Validasi
                                /*
                                while (counter_data_reset < int.Parse(ButtonNumPcs.Text))
                                {
                                    //integerList[integerList.Count - 1];
                                    Data_Measure_Result.Add(Data_Measure_Result[Data_Measure_Result.Count - 1]);
                                    counter_data = Data_Measure_Result.Count;
                                    counter_data_reset = counter_data_reset + 1;
                                }
                                */
                                //

                                #region theresholdfinal
                                int theresholdmax_counter = Data_Measure_Result.Count(x => float.Parse(x.Measures) > therehold_max);

                                textBox_theresholdmax.Invoke((Action)delegate
                                {
                                    //ListFilesToProcess.Count(item => item.IsChecked);
                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                    textBox_theresholdmax.Text = theresholdmax_counter.ToString();
                                });

                                int theresholdmin_counter = Data_Measure_Result.Count(x => float.Parse(x.Measures) < thereshold_min);

                                textBox_theresholdmin.Invoke((Action)delegate
                                {
                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                    textBox_theresholdmin.Text = (thereshold_min_counter).ToString();
                                });

                                // Thereshold min
                                #endregion

                                Curr_Kernel_TextBox.Invoke((Action)delegate
                                {
                                    //Curr_Kernel_TextBox.Text = (counter_data + 1).ToString();
                                    Curr_Kernel_TextBox.Text = counter_data_reset.ToString();
                                });
                                Measure_Cond = false;
                                countingbatch = false;
                            }

                            else
                            {
                                Console.WriteLine("Nilainya Readstr Dari if else null adalah: " + readStr);
                            }

                        }

                        else
                        {
                            Console.WriteLine("Nilainya Readstr null adalah: " + readStr);
                        }
                        //string input = "hello123world";
                        //bool isDigitPresent = input.Any(c => char.IsDigit(c));

                    }

                    #endregion

                    #region Get Aggregate value

                    //start_next_init = 0;
                    //OpenCon_Port_local(mySerialPort, BaudRate);
                    while (aggregate_cond)
                    {
                        Result_Parsing = string.Empty;
                        Console.WriteLine("Start Aggregate_cond");
                        Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);
                        Thread.Sleep(4000);// this solves the problem
                        readBuffer = new byte[mySerialPort.ReadBufferSize];
                        readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                        readStr = string.Empty;
                        readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        readStr = readStr.Trim();

                        Console.WriteLine("ReadStr Average adalah: " + readStr);
                        foreach (string s in charactersToReplace)
                        {
                            Result_Parsing = readStr.Replace(s, "");
                        }

                        //Result_Parsing = GetWords(Result_Parsing).FirstOrDefault();
                        if (Result_Parsing != null)
                        {
                            if (
                                Result_Parsing.Contains("-")
                                && (Result_Parsing.Length) > 4
                                && Result_Parsing.Contains(STX)
                                && Result_Parsing.Contains(ETX)
                                )
                            {
                                MyTimer.Enabled = false;
                                MyTimer.Stop();

                                AllText = GetWords(Result_Parsing);
                                int checkindex;
                                string aggregate_value_string = string.Empty;
                                foreach (string text in AllText)
                                {
                                    if (
                                        text.Length >= 10
                                        //&& text.Length <= 12
                                        && !text.Trim().ToLower().ToString().Contains("r")
                                        )
                                    {
                                        aggregate_value_string = text;
                                    }
                                }

                                Result_Parsing = aggregate_value_string.Substring(5, 3);
                                Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                    , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                Result_Parsing = (double.Parse(Result_Parsing) + bias_value).ToString("0.0");

                                Data_Avg_Result.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                aggregate_cond = false;



                                Curr_Measure_TextBox.Invoke((Action)delegate
                                {
                                    //Curr_Measure_TextBox.Text = Result_Parsing.Format("0.0");
                                    Curr_Measure_TextBox.Text = string.Format("{0:F1}", Result_Parsing) + "%";
                                });


                                total_average = 0;
                                Current_Avg_TextBox.Invoke((Action)delegate
                                {
                                    foreach (data_measure_2 average_val in Data_Avg_Result)
                                    {
                                        total_average = total_average + float.Parse(average_val.Measures);
                                    }

                                    total_current_Average = total_average / Data_Avg_Result.Count();
                                    Current_Avg_TextBox.Text = total_current_Average.ToString("0.00") + "%";
                                    //Final Average
                                });


                                //loat Result_Parsing_input = float.Parse(Result_Parsing);
                                Sensor_input_Helper.MySql_Insert_Measure(batch_id, 1000 + current_interval + 1, float.Parse(Result_Parsing), DateTime.Now, 1, 1);

                                Console.WriteLine("Finish Aggregate");
                                readStr = string.Empty;
                            }
                            else if (
                                        (Data_Measure_Result.Count == 0 && Result_Parsing.Substring(3, 5) == "00000")
                                        || (!Result_Parsing.Contains("-") && (Result_Parsing.Length) > 10)

                                        )
                            {
                                Result_Parsing = "0.0";

                                Data_Avg_Result.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                aggregate_cond = false;


                                Curr_Measure_TextBox.Invoke((Action)delegate
                                {
                                    Curr_Measure_TextBox.Text = Result_Parsing + "%";
                                });

                                Current_Avg_TextBox.Invoke((Action)delegate
                                {
                                    foreach (data_measure_2 average_val in Data_Avg_Result)
                                    {
                                        total_average = total_average + float.Parse(average_val.Measures);
                                    }

                                    total_current_Average = total_average / Data_Avg_Result.Count();
                                    Current_Avg_TextBox.Text = total_current_Average.ToString("0.0") + "%";
                                    //Final Average
                                });

                            }
                            else
                            {
                                Console.WriteLine("Aggreagte empty");
                            }


                        }

                        //start_next_init++;
                    }

                    #endregion Finish get aggregate value
                    Console.WriteLine("Finish aggregate region");

                    #region Finish All Measure and close port

                    Console.WriteLine("data_average count adalah: ", Data_Avg_Result.Count().ToString());
                    //Console.WriteLine("data_average count adalah: ", current_interval.ToString());

                    if (Data_Avg_Result.Count() == TotalInterval || bool_check_error == true)
                    {
                        stat_continue = false;
                        mySerialPort.DiscardInBuffer();
                        mySerialPort.DiscardOutBuffer();
                        stat_continue = false;
                        start_next_cond = false;
                        aggregate_cond = false;

                        next_action_button(bool_check_error);
                        finish_measurement = 1;



                    }


                    #endregion

                }
                catch (TimeoutException ex)
                {
                    stat_continue = false;
                    stat_continue = false;
                    start_next_cond = false;
                    aggregate_cond = false;
                    bool error = true;
                    next_action_button(error);
                    Thread.Sleep(10000);
                    Sensor_input_Helper.Update_ErrorCode(Sensor_input_Helper.GetLocalIPAddress(), batch_id, "011");
                    MessageBox.Show(this, "Error 011 - no message during checking for 5 mins");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Console.WriteLine(ex);
                    //return "";
                }

            }

            //MessageBox.Show("measurement finsih");
            Console.WriteLine("Measurement Finish");
        }


        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            int product_type = SensorHelper_2.Get_ProductType();
            if (product_type == 1)
            {
                this.Text = "Gii RX-30 Automatic In-line Interval Moisture Measurement System  (Ver 1.01)";
            }
            else if (product_type == 2)
            {
                this.Text = "Gii RX-30 Automatic In-line Interval Moisture Measurement System  (Ver 1.10)";
            }
            else //3
            {
                this.Text = "Gii RX-20 Automatic In-line Interval Moisture Measurement System  (Ver 1.01)";
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox_theresholdmin_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
