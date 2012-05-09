using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace SerialPortBridge
{
    class SerialPortBridge
    {
        bool status = false;
        private SerialPort left;
        private SerialPort right;

        private int baudrate;
        private Parity parity;
        private int databits;
        private StopBits stopbits;
        private Handshake handshake = Handshake.None;

        public SerialPortBridge()
        {
        }

        public string[] GetAvailablePorts() { return SerialPort.GetPortNames(); }

        public bool Enabled { get { return status; } }

        public void SetOptions(string baudrate, string parity, string databits, string stopbits)
        {
            int.TryParse(baudrate, out this.baudrate);
            if (parity == "None") this.parity = Parity.None;
            else if (parity == "Even") this.parity = Parity.Even;
            else if (parity == "Odd") this.parity = Parity.Odd;
            else if (parity == "Mark") this.parity = Parity.Mark;
            else if (parity == "Space") this.parity = Parity.Space;
            int.TryParse(databits, out this.databits);
            if (stopbits == "None") this.stopbits = StopBits.None;
            else if (stopbits == "1") this.stopbits = StopBits.One;
            else if (stopbits == "1.5") this.stopbits = StopBits.OnePointFive;
            else if (stopbits == "2") this.stopbits = StopBits.Two;
        }

        public void SetOptions(int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            this.baudrate = baudrate;
            this.parity = parity;
            this.databits = databits;
            this.stopbits = stopbits;
        }

        public void Start(string left, string right)
        {
            if (status) throw new Exception("Stop before starting!");
            if (left == null || left == "" || right == null || right == "") throw new ArgumentNullException("Invalid arguments!");


            if (left == right)
            {
                this.left = new SerialPort(left, baudrate, parity, databits, stopbits);
                this.left.Handshake = handshake;
                this.left.DataReceived += new SerialDataReceivedEventHandler(either_DataReceived);
            }
            else
            {
                this.left = new SerialPort(left, baudrate, parity, databits, stopbits);
                this.right = new SerialPort(right, baudrate, parity, databits, stopbits);
                this.left.Handshake = this.right.Handshake = handshake;
                this.left.DataReceived += new SerialDataReceivedEventHandler(either_DataReceived);
                this.right.DataReceived += new SerialDataReceivedEventHandler(either_DataReceived);
            }

            status = true;
        }

        void either_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort from = (SerialPort)sender;
            SerialPort to;
            if (from == this.left)
            {
                if (this.right == null)
                {
                    // send back
                    to = from;
                }
                else
                {
                    // send to right
                    to = this.right;
                }
            }
            else
            {
                // send to left
                to = this.left;
            }
            var buff = new byte[from.BytesToRead];
            from.Read(buff, 0, buff.Length);
            to.Write(buff, 0, buff.Length);
        }

        public void Stop()
        {
            if (!status) return;
            status = false;
            left.Close();
            left = null;
            if (right != null)
            {
                right.Close();
                right = null;
            }
        }
    }
}
