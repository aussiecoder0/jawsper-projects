using System;
using System.IO;
using System.Net;
using System.Text;

namespace SkyTraqCs
{
    partial class SkyTraq
    {
        public void ReadAGPSStatus(ref SkyTraqConfig config)
        {
            config.agps_hours_left = 0;
            config.agps_enabled = 0;

            var now = DateTime.UtcNow;

            var request = new SkyTraqPackage(
                SkyTraqCommand.SKYTRAQ_COMMAND_READ_AGPS_STATUS,
                (byte)(now.Year >> 8),
                (byte)now.Year,
                (byte)now.Month,
                (byte)now.Day,
                (byte)now.Hour,
                (byte)now.Minute,
                (byte)now.Second
            );

            if (ACK == WritePackageWithResponse(request))
            {
                var response = ReadNextPackage();
                if (response != null)
                {
                    config.agps_hours_left = BitConverter.ToUInt16(response.data, 1);
                    config.agps_enabled = response.data[3];
                }
            }
        }

        //private static readonly string ephermis_url = @"ftp://skytraq:skytraq@60.250.205.31/ephemeris/Eph.dat";
        private static readonly string ephermis_url = @"C:\Users\Jawsper\Downloads\Eph.dat";
        //private static readonly int agps_upload_blocksize = 8192;

        public void DownloadAndUpdateAGPS()
        {
            var rq = WebRequest.Create(ephermis_url);
            var rsp = rq.GetResponse();
            var buff = new byte[4096];
            using (var outstr = new MemoryStream())
            {
                using (var instr = rsp.GetResponseStream())
                {
                    int bytesRead;
                    while ((bytesRead = instr.Read(buff, 0, buff.Length)) > 0)
                    {
                        outstr.Write(buff, 0, bytesRead);
                    }
                }
                outstr.Position = 0;

                var oldbaud = serialPort.BaudRate;
                var newbaud = 115200;
                if (oldbaud != newbaud) this.SetSpeed(115200, false);
                this.OutputDisable();

                SendAGPSData(outstr.ToArray());

                this.OutputEnableNMEA();

                if (oldbaud != newbaud) this.SetSpeed(oldbaud, false);
            }
        }

        private void CalculateAGPSChecksum(byte[] data, out uint checkSumA, out uint checkSumB)
        {
            ulong sumA = 0, sumB = 0;
            uint counter = 0;

            for (int i = 0; i < data.Length; ++i)
            {
                sumA += data[i];
                if (counter < 0x10000)
                {
                    sumB += data[i];
                    counter++;
                }
            }

            checkSumA = (uint)(sumA % 256);
            checkSumB = (uint)(sumB % 256);
        }

        private void SendAGPSData(byte[] data)
        {
            uint checkSumA, checkSumB;
            CalculateAGPSChecksum(data, out checkSumA, out checkSumB);

            var request = new SkyTraqPackage(1);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_SEND_AGPS_DATA;
            if (ACK == WritePackageWithResponse(request))
            {
                System.Threading.Thread.Sleep(500);

                var info_string = String.Format("BINSIZE = {0} Checksum = {1} Checksumb = {2} \0", data.Length, checkSumA, checkSumB);
                serialPort.Write(info_string);

                var buff = new byte[50];
                int c = serialPort.Read(buff, 0, buff.Length);
                var str = Encoding.ASCII.GetString(buff, 0, 2);
            }
        }
    }
}