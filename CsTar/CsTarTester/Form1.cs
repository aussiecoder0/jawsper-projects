using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace jawsper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OpenTar(@"e:\downloads\Gingerbread-GT-I9000-XXJVO-stock-kernel+hacks.tar");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                using (var tar = new CsTar(new FileStream(openFileDialog1.FileName, FileMode.Open)))
                {
                    OpenTar(openFileDialog1.FileName);
                }
            }
        }

        private void OpenTar(string file)
        {
            using (var tar = new CsTar(new FileStream(file, FileMode.Open)))
            {

            }

        }
    }
}
