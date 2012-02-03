using System;
using System.Diagnostics;
using System.IO;

namespace SkyTraqCs
{
    partial class SkyTraq
    {
        public int ReadDataloggerConfig(out SkyTraqConfig config)
        {
            int result = ERROR;
            config = null;

            var request = new SkyTraqPackage(SkyTraqCommand.SKYTRAQ_COMMAND_GET_CONFIG);
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

        public void WriteDataloggerConfig(SkyTraqConfig config)
        {
            WritePackageWithResponse(config.GetWritePackage());
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

            var request = new SkyTraqPackage(
                SkyTraqCommand.SKYTRAQ_COMMAND_CONFIGURE_MESSAGE_TYPE,
                what, // type
                0     // 1 = permanent
            );

            success = WritePackageWithResponse(request);

            return success;
        }

        public void ClearDatalog()
        {
            var request = new SkyTraqPackage(SkyTraqCommand.SKYTRAQ_COMMAND_ERASE);
            WritePackageWithResponse(request);
        }

        public long ReadDataLogSectorBatch(byte start_sector, byte end_sector, out byte[] buffer)
        {
            if (end_sector <= start_sector)
            {
                Debug.WriteLine("Incorrect parameters");
                buffer = null;
                return ERROR;
            }
            buffer = new byte[4096 * (end_sector - start_sector + 1)];
            var request = new SkyTraqPackage(
                SkyTraqCommand.SKYTRAQ_COMMAND_READ_BATCH,
                0, start_sector,
                0, end_sector
            );
            if (ACK == WritePackageWithResponse(request))
            {
                long i, len, count = 0;
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
                for (i = 0; i < 10; ++i)
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
                    Console.WriteLine("Wrong checksum for sectors {0}-{1}", start_sector, end_sector);
                    return ERROR;
                }
            }

            return ERROR;
        }

        public int ReadDataLogSector(byte sector, ref byte[] buffer)
        {
            if (buffer != null)
            {
                var request = new SkyTraqPackage(
                    SkyTraqCommand.SKYTRAQ_COMMAND_READ_SECTOR,
                    sector
                    );
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
                    for (i = 0; i < 10; ++i)
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
                    var buff = new byte[4096];
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

        public void ExportRawDataLog(Stream s)
        {
            SkyTraqConfig cfg;
            this.ReadDataloggerConfig(out cfg);

            int oldbaudrate = serialPort.BaudRate;
            this.SetSpeed(115200, false);

            using (var sw = new StreamWriter(s))
            {
                //sw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
                //sw.Write("<gpx xmlns=\"http://www.topografix.com/GPX/1/0\" creator=\"skytraq-datalogger\" version=\"1.0\">\n");
                //sw.Write("<trk>\n<trkseg>\n");

                int used_sectors = cfg.total_sectors - cfg.sectors_left + 1;
                used_sectors = 2;
                Console.WriteLine("Total used sectors: {0:000}/{1:000}", used_sectors, cfg.total_sectors);
                byte[] buffer;
                this.ReadDataLogSectorBatch(0, (byte)(used_sectors - 1U), out buffer);
                s.Write(buffer, 0, buffer.Length);

                for (byte i = 0; i < used_sectors; ++i)
                {
                    Console.Write("Sector {0:000}/{1:000} ({2:0.00}%)", i, used_sectors, ((double)i * 100.0 / (double)used_sectors));
                    var buff = new byte[4096];
                    Console.Write(" - start read");
                    this.ReadDataLogSector(i, ref buff);
                    Console.Write(" - end read");
                    s.Write(buff, 0, buff.Length);
                    Console.Write(" - start sleep");
                    System.Threading.Thread.Sleep(500);
                    Console.WriteLine(" - end sleep");
                }
            }

            this.SetSpeed(oldbaudrate, false);
        }


    }
}