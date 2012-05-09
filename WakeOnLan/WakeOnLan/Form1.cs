using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace WakeOnLan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mac_str = textBox1.Text; // add sanity check here
            var buff = new byte[6];
            for (int i = 0; (2 * i) < mac_str.Length; i++)
            {
                buff[i] = Convert.ToByte(mac_str.Substring(2 * i, 2), 16);
            }
            WOL(mtxtIP.Text, int.Parse(mtxtPort.Text), buff);
        }

        void WOL(string ip, int port, byte[] mac)
        {
            var packet = new byte[102];
            int i = 0;
            for (i = 0; i < 6; i++)
            {
                packet[i] = 0xFF;
            }
            for (i = 6; i < 102; i++)
            {
                packet[i] = mac[i % 6];
            }

            var sock = new UdpClient();
            var remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            sock.Send(packet, packet.Length, remoteEP);
        }
    }
}
