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
    public partial class TrackBarWithTimeLabels : UserControl
    {
        public event EventHandler DragComplete;
        bool dragging = false;
        public TrackBarWithTimeLabels()
        {
            InitializeComponent();
            UpdateLabels();
        }
        public int Value
        {
            set { if (!dragging && value < Maximum) { trackBar1.Value = value; } }
            get { return trackBar1.Value; }
        }
        public int Maximum
        {
            set { trackBar1.Maximum = value; UpdateLabels(); }
            get { return trackBar1.Maximum; }
        }

        private void UpdateLabels()
        {
            if (Value >= 0)
            {
                int minsec = Value;
                int min = minsec / 60, sec = minsec % 60;
                label1.Text = string.Format("{0:00}:{1:00}", min, sec);
            }
            if (Maximum >= 0)
            {
                int minsec = Maximum;
                int min = minsec / 60, sec = minsec % 60;
                label2.Text = string.Format("{0:00}:{1:00}", min, sec);
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            if (DragComplete != null) DragComplete(this, new EventArgs());
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }
    }
}
