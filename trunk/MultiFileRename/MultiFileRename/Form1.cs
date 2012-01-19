using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace EpisodeRename
{
    public partial class Form1 : Form
    {
        private DirectoryInfo folder;
        public Form1()
        {
            InitializeComponent();

            folder = new DirectoryInfo(@"\\bami\Downloads\TV\");
            txtFilePattern.Text = @"ntfs.+\.avi$";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                folder = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
            }
        }

        private List<FileInfo> GetFiles(DirectoryInfo dir, Regex dir_regex, Regex file_regex)
        {
            var list = new List<FileInfo>();
            foreach (var d in dir.GetDirectories().OrderBy(x => x.Name))
            {
                if (dir_regex == null || dir_regex.IsMatch(d.Name)) list.AddRange(GetFiles(d, dir_regex, file_regex));
            }
            foreach (var f in dir.GetFiles().OrderBy(x => x.Name))
            {
                if (file_regex == null || file_regex.IsMatch(f.Name)) list.Add(f);
            }
            return list;//.OrderBy(x => x.Name).ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(folder is DirectoryInfo) || !folder.Exists) return;
            var dir_rx = txtFolderPattern.Text.Length == 0 ? null : new Regex(txtFolderPattern.Text, RegexOptions.IgnoreCase);
            var file_rx = txtFilePattern.Text.Length == 0 ? null : new Regex(txtFilePattern.Text, RegexOptions.IgnoreCase);
            var files = GetFiles(folder, dir_rx, file_rx);
            panel1.Controls.Clear();
            var y = 0;
            foreach (var f in files)
            {
                var tb = new TextBox();
                tb.Size = new Size(500, 20);
                tb.Tag = f;
                tb.Text = f.Name;
                tb.Location = new Point(0, y);
                y += 20;
                panel1.Controls.Add(tb);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var tb in panel1.Controls.OfType<TextBox>())
            {
                var fi = tb.Tag as FileInfo;
                var new_name = tb.Text;
                if (fi.Name.Equals(new_name)) continue;
                var dir = fi.DirectoryName;
                fi.MoveTo(dir + "\\" + new_name);
            }
        }
    }
}
