using System;
using SkyTraqCs;

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
            //st.ReadSoftwareVersion();
            //SkyTraqConfig cfg;
            //st.ReadDataloggerConfig(out cfg);
            //st.ReadAGPSStatus(ref cfg);
            //cfg.datalog_enable = 1;
            //st.WriteDataloggerConfig(cfg);

            //st.ClearDatalog();

            /*using (var fs = new FileStream(string.Format("output-{0:yyyyMMddThhmmss}.txt", DateTime.Now), FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                st.ExportDataLog(fs);
            }*/


            //st.OutputEnableNMEA();

            //st.DownloadAndUpdateAGPS();

            bool trueish = true;
            while (trueish)
            {
                var str = st.GetNMEAMessage();
                var items = st.ParseNMEAMessage(str);
                if (items != null)
                {
                    if (items.ContainsKey("utc_time"))
                    {
                        Console.WriteLine("Time: {0:hh:mm:ss}", items["utc_time"]);
                    }
                }
            }

            st.CloseDevice();

            //Console.ReadLine();
        }
    }
}
