using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerPage
{
    static class Global
    {
        private static string _globalVar = "";
        public static string GlobalVar
        {
            get { return _globalVar; }
            set { _globalVar = value; }
        }
        private static string _globalVar1 = "Current Interval Moisture Measurement";
        public static string GlobalVar1
        {
            get { return _globalVar1; }
            set { _globalVar1 = value; }
        }
        private static string _globalVar2 = "Interval Number";
        public static string GlobalVar2
        {
            get { return _globalVar2; }
            set { _globalVar2 = value; }
        }
        private static string _globalVar3 = "Kernel Counter";
        public static string GlobalVar3
        {
            get { return _globalVar3; }
            set { _globalVar3 = value; }
        }
        private static string _globalVar4 = "Average Moisture";
        public static string GlobalVar4
        {
            get { return _globalVar4; }
            set { _globalVar4 = value; }
        }
        private static string _globalVar5 = "Product";
        public static string GlobalVar5
        {
            get { return _globalVar5; }
            set { _globalVar5 = value; }
        }
        private static string _globalVar6 = "No. of Intervals";
        public static string GlobalVar6
        {
            get { return _globalVar6; }
            set { _globalVar6 = value; }
        }
        private static string _globalVar7 = "PCS per Interval";
        public static string GlobalVar7
        {
            get { return _globalVar7; }
            set { _globalVar7 = value; }
        }
        private static string _globalVar8 = "Int. Waiting Time";
        public static string GlobalVar8
        {
            get { return _globalVar8; }
            set { _globalVar8 = value; }
        }
        private static string _globalVar9 = "Options";
        public static string GlobalVar9
        {
            get { return _globalVar9; }
            set { _globalVar9 = value; }
        }
        private static string _globalVar10 = "Sensor Number";
        public static string GlobalVar10
        {
            get { return _globalVar10; }
            set { _globalVar10 = value; }
        }
        private static string _globalVar11 = "Threshold Max = 00%";
        public static string GlobalVar11
        {
            get { return _globalVar11; }
            set { _globalVar11 = value; }
        }
        private static string _globalVar12 = "Threshold Min = 00%";
        public static string GlobalVar12
        {
            get { return _globalVar12; }
            set { _globalVar12 = value; }
        }
    }
}
