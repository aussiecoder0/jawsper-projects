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
    public partial class PanelNowPlaying : UserControl
    {
        public PanelNowPlaying()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return "Now playing";
            }
            set
            {
                base.Text = value;
            }
        }

        public string Title { set { txtTitle.Text = value; } }
        public string Artist { set { txtArtist.Text = value; } }
        public string Album { set { txtAlbum.Text = value; } }
    }
}
