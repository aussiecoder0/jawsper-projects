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
    public partial class TrackInfo : UserControl
    {
        public TrackInfo()
        {
            InitializeComponent();
        }
        public string Track
        {
            get { return lblTrack.Text; }
            set { lblTrack.Text = value; }
        }
        public string Artist
        {
            get { return lblArtist.Text; }
            set { lblArtist.Text = value; }
        }
    }
}
