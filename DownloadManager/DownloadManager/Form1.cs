using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DownloadManager
{
    public partial class Form1 : Form
    {
        Downloader d;
        public Form1()
        {
            InitializeComponent();
            d = new Downloader();
            d.DownloadStartEvent += new DownloadStartEventHandler(d_DownloadStartEvent);
            d.DownloadProgressEvent += new DownloadProgressEventHandler(d_DownloadProgressEvent);
            d.DownloadCompleteEvent += new DownloadCompleteEventHandler(d_DownloadCompleteEvent);
            d.Start();
        }

        void d_DownloadStartEvent(string name)
        {
            Invoke((MethodInvoker)delegate()
            {
                label1.Text = name;
            });
        }

        void d_DownloadProgressEvent(long size, double progress)
        {
            Invoke((MethodInvoker)delegate()
            {
                if (progress < 0)
                {
                    progressBar1.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    progressBar1.Value = (int)progress;
                }
                label3.Text = size.ToString();
            });
        }

        void d_DownloadCompleteEvent(long size, TimeSpan time)
        {
            Invoke((MethodInvoker)delegate()
            {
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Value = 0;
                label2.Text = string.Format("Size: {0}, Time: {1}", size, time);
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            d.Abort();
            base.OnClosing(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var uri = new Uri(textBox1.Text);
            var dest = new DirectoryInfo(".\\Downloads");
            if (!dest.Exists) dest.Create();
            var item = new DownloadItem(uri, dest);
            if (textBox2.Text.Length > 0)
            {
                item.Referer = textBox2.Text;
                textBox2.Text = "";
            }
            d.Add(item);
            textBox1.Text = "";
        }
    }
}
