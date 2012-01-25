using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.IO;

/*

    Control program for SkyTraq GPS data logger.

    Copyright (C) 2008  Jesper Zedlitz, jesper@zedlitz.de
    Copyright (C) 2012  Jasper Seidel, conversion to C#

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111 USA

 */

namespace SkyTraqCs
{
    public class SkyTraq
    {
        private SerialPort serialPort;

        private readonly int ERROR = -1;
        private readonly int SUCCESS = 0;
        private readonly int NACK = -1;
        private readonly int ACK = 0;

        private readonly double TIMEOUT = 20.001;

        public void OpenDevice(string device, int baud)
        {
            serialPort = new SerialPort(device, baud, Parity.None, 8, StopBits.One);
            serialPort.Handshake = Handshake.None;
            serialPort.Open();
        }
        public void CloseDevice()
        {
            serialPort.Close();
        }

        #region DataLogger

        public int ReadSoftwareVersion()
        {
            int result = ERROR;
            var request = new SkyTraqPackage(2);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_QUERY_SOFTWARE_VERSION;
            request.data[1] = 1;

            if( ACK == WritePackageWithResponse(request) )
            {
                var response = ReadNextPackage();
                if (response != null)
                {
                    var printf = String.Format("Kernel version: {0}.{1}.{2} -- ODM version: {3}.{4}.{5} -- revision: 20{6:00}-{7:00}-{8:00}",
                        response.data[3], response.data[4], response.data[5],
                        response.data[7], response.data[8], response.data[9],
                        response.data[11], response.data[12], response.data[13]
                        );
                    Console.WriteLine(printf);

                    result = SUCCESS;
                }
            }

            return result;
        }

        public int ReadDataloggerConfig(out SkyTraqConfig config)
        {
            int result = ERROR;
            config = null;

            var request = new SkyTraqPackage(1);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_GET_CONFIG;
            if (ACK == WritePackageWithResponse(request))
            {
                var response = ReadNextPackage();
                if (response != null)
                {
                    config = new SkyTraqConfig(response.data);
                    result = SUCCESS;
                }
            }

            return result;
        }
        void cpy(byte[] dst, int pos, byte[] src)
        {
            int j = pos;
            for (int i = src.Length - 1; i >= 0; --i)
            {
                dst[j++] = src[i];
            }
        }
        public void WriteDataloggerConfig(SkyTraqConfig config)
        {
            var request = new SkyTraqPackage(27);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_WRITE_CONFIG;
            cpy(request.data, 1, BitConverter.GetBytes(config.max_time));
            cpy(request.data, 5, BitConverter.GetBytes(config.min_time));
            cpy(request.data, 9,BitConverter.GetBytes(config.max_distance));
            cpy(request.data, 13,BitConverter.GetBytes(config.min_distance));
            cpy(request.data, 17,BitConverter.GetBytes(config.max_speed));
            cpy(request.data, 21,BitConverter.GetBytes(config.min_speed));
            request.data[25] = config.datalog_enable;
            request.data[26] = config.log_fifo_mode;
            WritePackageWithResponse(request);
        }

        public void ReadAGPSStatus(ref SkyTraqConfig config)
        {
            config.agps_hours_left = 0;
            config.agps_enabled = 0;

            var now = DateTime.UtcNow;

            var request = new SkyTraqPackage(8);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_READ_AGPS_STATUS;
            request.data[1] = (byte)((now.Year >> 8) & 0xFF);
            request.data[2] = (byte)(now.Year & 0xFF);
            request.data[3] = (byte)now.Month;
            request.data[4] = (byte)now.Day;
            request.data[5] = (byte)now.Hour;
            request.data[6] = (byte)now.Minute;
            request.data[7] = (byte)now.Second;

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


        public int OutputDisable()
        {
            return SetOutput(0);
        }
        public int OutputEnableNMEA()
        {
            return SetOutput(1);
        }
        public int OutputEnableBinary()
        {
            return SetOutput(2);
        }

        private int SetOutput(byte what)
        {
            int success;

            var request = new SkyTraqPackage(3);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_CONFIGURE_MESSAGE_TYPE;
            request.data[1] = what; // type
            request.data[2] = 0;    // 1 = permanent

            success = WritePackageWithResponse(request);

            return success;
        }

        public void ClearDatalog()
        {
            var request = new SkyTraqPackage(1);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_ERASE;
            WritePackageWithResponse(request);
        }

        public int ReadDataLogSector(byte sector, ref byte[] buffer)
        {
            if (buffer != null)
            {
                var request = new SkyTraqPackage(2);
                request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_READ_SECTOR;
                request.data[1] = sector;
                if (ACK == WritePackageWithResponse(request))
                {
                    int i, len, count = 0;
                    byte c, lastByte1 = 0, lastByte2 = 0, cs, checksum;

                    Debug.WriteLine("START READING DATA");

                    cs = 0;
                    len = 1;
                    c = ReadByte();
                    while (len > 0)
                    {
                        if ((lastByte2 == (byte)'E') && lastByte1 == (byte)'N' && c == (byte)'D')
                        {
                            cs = (byte)(cs ^ (byte)'N');
                            cs = (byte)(cs ^ (byte)'E');

                            Debug.WriteLine("DONE");
                            break;
                        }

                        if (count >= 2)
                        {
                            buffer[count - 2] = lastByte2;
                        }

                        cs = (byte)(cs ^ c);

                        lastByte2 = lastByte1;
                        lastByte1 = c;
                        count++;
                        len = 1;
                        c = ReadByte();
                    }

                    /* remaining characters after data block */
                    for( i = 0; i < 10; ++i)
                    {
                        ReadByte();
                    }

                    checksum = ReadByte();

                    for (i = 0; i < 5; ++i)
                    {
                        ReadByte();
                    }

                    if (cs == checksum) return count - 2;
                    else
                    {
                        Console.WriteLine("Wrong checksum for sector {0}", sector);
                        return ERROR;
                    }
                }
            }

            return ERROR;
        }

        public void SetSpeed(int baudrate, bool permanent)
        {
            byte speed = byte.MaxValue;
            switch (baudrate)
            {
                case 4800: speed = 0;break;
                case 9600: speed = 1; break;
                case 19200: speed = 2; break;
                case 38400: speed = 3; break;
                case 57600: speed = 4; break;
                case 115200: speed = 5; break;
                default: Debug.WriteLine("Baudrate {0} is not supported by device!", baudrate); return;
            }
            var request = new SkyTraqPackage(4);
            request.data[0] = SkyTraqCommand.SKYTRAQ_COMMAND_CONFIGURE_SERIAL_PORT;
            request.data[1] = 0;
            request.data[2] = speed;
            request.data[3] = (byte)(permanent ? 1 : 0);
            if (ACK == WritePackageWithResponse(request))
            {
                serialPort.BaudRate = baudrate;
            }
            else
            {
                Debug.WriteLine("setting line speed FAILED");
            }
        }

        #endregion

        #region LowLevel

        private int WritePackageWithResponse(SkyTraqPackage p)
        {
            int retries_left = 3;
            byte request_message_id;

            request_message_id = p.data[0];
            WriteSkyTraqPackage(p);

            Debug.WriteLine("Waiting for ACK with msg id 0x{0:x2}", request_message_id);

            while (retries_left > 0)
            {
                var response = ReadNextPackage();

                if (response != null)
                {
                    if (response.data[0] == SkyTraqCommand.SKYTRAQ_RESPONSE_ACK)
                    {
                        Debug.WriteLine("got ACK for msg id: 0x{0:x2}", response.data[1]);
                        if (response.data[1] == request_message_id) return ACK;
                    }
                    else if (response.data[0] == SkyTraqCommand.SKYTRAQ_RESPONSE_NACK)
                    {
                        Debug.WriteLine("got NACK for msg id: 0x{0:x2}", response.data[1]);
                        if (response.data[1] == request_message_id) return NACK;
                    }
                }
                else
                {
                    Debug.WriteLine("Got invalid package");
                }

                retries_left--;
            }

            return ERROR;
        }

        private SkyTraqPackage ReadNextPackage()
        {
            byte c, lastByte = 0;
            var start = DateTime.Now;

            serialPort.ReadTimeout = (int)(TIMEOUT * 1000);

            try
            {
                c = ReadByte();
                while ((DateTime.Now - start).TotalSeconds < TIMEOUT)
                {
                    if ((lastByte == 0xa0) && c == 0xa1)
                    {
                        var pkg = new SkyTraqPackage();
                        int dataRead;
                        byte end1, end2;

                        pkg.Resize(ReadUShort());
                        dataRead = 0;
                        while (dataRead < pkg.length)
                        {
                            pkg.data[dataRead++] = ReadByte();
                        }
                        pkg.checksum = ReadByte();
                        end1 = ReadByte();
                        end2 = ReadByte();

                        if (end1 == 0x0d && end2 == 0x0a)
                        {
                            if (pkg.CheckChecksum())
                            {
                                return pkg;
                            }
                        }
                    }

                    lastByte = c;

                    c = ReadByte();
                }
            }
            catch (System.ServiceProcess.TimeoutException)
            {
                return null;
            }
            return null;
        }

        private byte ReadByte()
        {
            try
            {
                byte b = (byte)serialPort.ReadByte();
                //Debug.WriteLine("{0:x2} {1}", b, (char)b);
                return b;
            }
            catch (System.ServiceProcess.TimeoutException e)
            {
                throw e;
            }
        }

        private ushort ReadUShort()
        {
            ushort output = 0;
            var tmp = new byte[2];
            tmp[1] = ReadByte();
            tmp[0] = ReadByte();
            output = BitConverter.ToUInt16(tmp, 0);
            return output;
        }

        private void WriteSkyTraqPackage(SkyTraqPackage p)
        {
            var data = new byte[7 + p.length];

            int j = 0;
            data[j++] = 0xa0;
            data[j++] = 0xa1;
            data[j++] = (byte)((p.length >> 8) & 0xFF);
            data[j++] = (byte)(p.length & 0xFF);
            for (int i = 0; i < p.length; ++i)
            {
                data[j++] = p.data[i];
            }
            data[j++] = p.CalculateSkyTraqChecksum();
            data[j++] = 0x0d;
            data[j++] = 0x0a;

            serialPort.Write(data, 0, data.Length);
        }


        #endregion

        public void ExportDataLog(Stream s)
        {
            SkyTraqConfig cfg;
            this.ReadDataloggerConfig(out cfg);

            int oldbaudrate = serialPort.BaudRate;
            this.SetSpeed(115200, false);

            using (var sw = new StreamWriter(s))
            {
                sw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
                sw.Write("<gpx xmlns=\"http://www.topografix.com/GPX/1/0\" creator=\"skytraq-datalogger\" version=\"1.0\">\n");
                sw.Write("<trk>\n<trkseg>\n");

                int used_sectors = cfg.total_sectors - cfg.sectors_left + 1;
                long last_timestamp = 0;
                Console.WriteLine("Total used sectors: {0:000}/{1:000}", used_sectors, cfg.total_sectors);
                for (byte i = 0; i < used_sectors; ++i)
                {
                    Console.Write("Sector {0:000}/{1:000} ({2:0.00}%)", i, used_sectors, ((double)i * 100.0 / (double)used_sectors));
                    var buff = new byte[4100];
                    Console.Write(" - start read");
                    this.ReadDataLogSector(i, ref buff);
                    Console.Write(" - end read");
                    last_timestamp = DatalogDecode.ProcessBuffer(sw, buff, last_timestamp);
                    Console.Write(" - start sleep");
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine(" - end sleep");
                }
                sw.Write("</trkseg>\n</trk>\n</gpx>\n");
            }

            this.SetSpeed(oldbaudrate, false);
        }
    }
}
