using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkyTraqCs;
using System.IO;

namespace SkyTraqTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var st = new SkyTraq();
            st.OpenDevice("COM3", 38400);
            //st.SetSpeed(38400, false);
            //st.OutputDisable();
            st.ReadSoftwareVersion();
            SkyTraqConfig cfg;
            st.ReadDataloggerConfig(out cfg);
            //cfg.datalog_enable = 1;
            //st.WriteDataloggerConfig(cfg);

            //st.ClearDatalog();

            /*using (var fs = new FileStream(string.Format("output-{0:yyyyMMddThhmmss}.txt", DateTime.Now), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                st.ExportDataLog(fs);
            }*/


            //st.OutputEnableNMEA();

            st.DownloadAndUpdateAGPS();

            st.CloseDevice();

            //Console.ReadLine();
        }
    }
}
