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
        private Regex folderPattern;
        private Regex filePattern;

        public Form1()
        {
            InitializeComponent();

            SetFolder(@"Y:\TV\sb\");
            SetPattern(@"\.(mkv|avi)$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
            {
                SetFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        void SetFolder(String path)
        {
            folderBrowserDialog1.SelectedPath = path;
            folder = new DirectoryInfo(path);
            label1.Text = path;
        }

        void SetPattern(String pattern)
        {
            txtFilePattern.Text = pattern;
        }

        private List<FileInfo> GetFiles(DirectoryInfo dir)
        {
            var list = new List<FileInfo>();
            foreach (var d in dir.GetDirectories().OrderBy(x => x.Name))
            {
                if (folderPattern == null || folderPattern.IsMatch(d.Name)) list.AddRange(GetFiles(d));
            }
            foreach (var f in dir.GetFiles().OrderBy(x => x.Name))
            {
                if (filePattern == null || filePattern.IsMatch(f.Name)) list.Add(f);
            }
            return list;//.OrderBy(x => x.Name).ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(folder is DirectoryInfo) || !folder.Exists) return;

            folderPattern = txtFolderPattern.Text.Length == 0 ? null : new Regex(txtFolderPattern.Text, RegexOptions.IgnoreCase);
            filePattern = txtFilePattern.Text.Length == 0 ? null : new Regex(txtFilePattern.Text, RegexOptions.IgnoreCase);

            var files = GetFiles(folder);
            panel1.Controls.Clear();
            var y = 0;
            foreach (var f in files)
            {
                var tb = new TextBox();
                tb.Size = new Size(500, 20);
                tb.Tag = f;
                tb.Text = f.Name;
                tb.Location = new Point(0, y);
                tb.KeyDown += new KeyEventHandler(tb_KeyDown);
                y += 20;
                panel1.Controls.Add(tb);
            }
        }

        void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                var current = sender as TextBox;
                var tb = panel1.GetNextControl(current, e.KeyCode == Keys.Down)as TextBox;
                if (tb != null)
                {
                    tb.SelectionStart = current.SelectionStart;
                    tb.Focus();
                }
                else
                {
                    e.SuppressKeyPress = true;
                }
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var tb in panel1.Controls.OfType<TextBox>())
            {
                if (tb.Tag is FileInfo)
                {
                    var fi = tb.Tag as FileInfo;
                    var new_name = tb.Text;
                    if (fi.Name.Equals(new_name)) continue;
                    var dir = fi.DirectoryName;
                    fi.MoveTo(dir + Path.DirectorySeparatorChar + new_name);
                }
            }
        }
    }
}
