using System;
using System.Diagnostics;
using System.IO.Ports;

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
    public partial class SkyTraq
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
            serialPort.NewLine = "\r\n";
            serialPort.Open();
        }
        public void CloseDevice()
        {
            serialPort.Close();
        }

        public void SetSpeed(int baudrate, bool permanent)
        {
            byte speed = byte.MaxValue;
            switch (baudrate)
            {
                case 4800: speed = 0; break;
                case 9600: speed = 1; break;
                case 19200: speed = 2; break;
                case 38400: speed = 3; break;
                case 57600: speed = 4; break;
                case 115200: speed = 5; break;
                default: Debug.WriteLine("Baudrate {0} is not supported by device!", baudrate); return;
            }
            var request = new SkyTraqPackage(SkyTraqCommand.SKYTRAQ_COMMAND_CONFIGURE_SERIAL_PORT, 0, speed, (byte)(permanent ? 1 : 0));
            if (ACK == WritePackageWithResponse(request))
            {
                serialPort.BaudRate = baudrate;
            }
            else
            {
                Debug.WriteLine("setting line speed FAILED");
            }
        }

        public int ReadSoftwareVersion()
        {
            int result = ERROR;
            var request = new SkyTraqPackage(SkyTraqCommand.SKYTRAQ_COMMAND_QUERY_SOFTWARE_VERSION, 1);

            if (ACK == WritePackageWithResponse(request))
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
    }
}