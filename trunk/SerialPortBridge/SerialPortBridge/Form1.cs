using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SerialPortBridge
{
    public partial class Form1 : Form
    {
        private SerialPortBridge bridge;

        public Form1()
        {
            InitializeComponent();
            bridge = new SerialPortBridge();
            var ports = bridge.GetAvailablePorts();
            cmbSerialPort1.Items.AddRange(ports);
            cmbSerialPort2.Items.AddRange(ports);
            cmbDatabits.SelectedItem = "8";
            cmbParity.SelectedItem = "None";
            cmbStopbits.SelectedItem = "1";
            cmbFlowcontrol.SelectedItem = "None";
        }

        void UpdateUI()
        {
            cmbSerialPort1.Enabled = cmbSerialPort2.Enabled = !bridge.Enabled;
            btnEnableBridge.Enabled = grpSettings.Enabled = !bridge.Enabled;
            btnDisableBridge.Enabled = bridge.Enabled;
        }

        private void btnEnableBridge_Click(object sender, EventArgs e)
        {
            try
            {
                bridge.SetOptions(txtBaudrate.Text, cmbParity.Text, cmbDatabits.Text, cmbStopbits.Text);
                bridge.Start(cmbSerialPort1.Text, cmbSerialPort2.Text);
            }
            catch { }
            UpdateUI();
        }

        private void btnDisableBridge_Click(object sender, EventArgs e)
        {
            bridge.Stop();
            UpdateUI();
        }
    }
}
