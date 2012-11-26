using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPCdotNet.Client
{
    public partial class Form1 : Form
    {
        private MPC mpc = new MPC();

        public Form1()
        {
            InitializeComponent();

            mpc.Open(Properties.Settings.Default.Server, Properties.Settings.Default.Port);

            foreach (var kv in mpc.GetStatus())
            {
                Console.WriteLine("{0} = {1}", kv[0], kv[1]);
            }

            /*foreach (var kv in mpc.GetStats())
            {
                Console.WriteLine("{0} = {1}", kv[0], kv[1]);
            }*/

            /*foreach (var output in mpc.GetOutputs())
            {
                Console.WriteLine("{0}", output);
            }*/

            /*foreach (var kv in mpc.GetCommands())
            {
                Console.WriteLine("{0} = {1}", kv[0], kv[1]);
            }*/
            foreach (var kv in mpc.GetCurrentSong())
            {
                Console.WriteLine("{0} = {1}", kv[0], kv[1]);
            }

            mpc.SendCommand("currentsong");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                mpc.Close();
            }
            catch (ObjectDisposedException)
            { }
        }
    }
}
