using System;
using System.Diagnostics;
namespace SkyTraqCs
{
    partial class SkyTraq
    {
        private int WritePackageWithResponse(SkyTraqPackage p)
        {
            int retries_left = 3;
            byte request_message_id;

            request_message_id = p.data[0];
            WritePackage(p);

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

            Debug.WriteLine("Ran out of retries waiting for response of msg id 0x{0:x2}", request_message_id);
            return ERROR;
        }

        private SkyTraqPackage ReadNextPackage()
        {
            byte c, prevByte = 0;
            var start = DateTime.Now;

            serialPort.ReadTimeout = (int)(TIMEOUT * 1000);

            try
            {
                c = ReadByte();
                while ((DateTime.Now - start).TotalSeconds < TIMEOUT)
                {
                    if ((prevByte == 0xa0) && c == 0xa1)
                    {
                        int dataRead;
                        byte end1, end2;

                        var pkg = new SkyTraqPackage(ReadUShort());

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

                    prevByte = c;

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

        private void WritePackage(SkyTraqPackage p)
        {
            var data = p.GetRawPackage();
            serialPort.Write(data, 0, data.Length);
        }
    }
}