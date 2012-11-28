using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPCdotNet.Client
{
    public partial class PanelRawCommand : UserControl
    {
        public PanelRawCommand()
        {
            InitializeComponent();
        }

        public MPC MPC { set; private get; }

        private void txtCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    var results = MPC.SendRawCommand(txtCommand.Text);
                    txtResults.Text = results != null ? string.Join("\r\n", results.Select(o => o.Key + ": " + o.Value)) : "";
                }
                catch (MPCException ex)
                {
                    txtResults.Text = ex.Message;
                }
                txtCommand.Text = "";
            }
        }

    }
}
