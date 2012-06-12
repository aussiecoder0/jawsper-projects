using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DownloadManager
{
    public delegate void DownloadStartEventHandler(string name);
    public delegate void DownloadProgressEventHandler(long size, double progress);
    public delegate void DownloadCompleteEventHandler(long size, TimeSpan time);

    class Downloader
    {
        public event DownloadStartEventHandler DownloadStartEvent;
        public event DownloadProgressEventHandler DownloadProgressEvent;
        public event DownloadCompleteEventHandler DownloadCompleteEvent;

        private Thread t;
        private DownloadQueue q;

        public Downloader()
        {
            q = new DownloadQueue();
            t = new Thread(Run);
        }
        public void Start()
        {
            if (DownloadStartEvent == null) throw new ArgumentNullException();
            if (DownloadProgressEvent == null) throw new ArgumentNullException();
            if (DownloadCompleteEvent == null) throw new ArgumentNullException();

            t.Start();
        }
        public void Abort()
        {
            t.Abort();
        }

        public void Add(DownloadItem item)
        {
            q.Enqueue(item);
        }

        private void Run()
        {
            try
            {
                while (true)
                {
                    if (q.Count > 0)
                    {
                        var i = q.Dequeue();
                        i.DownloadStartEvent += (n) => DownloadStartEvent(n);
                        i.DownloadProgressEvent += (s,p) => DownloadProgressEvent(s,p);
                        i.DownloadCompleteEvent += (s,t) => DownloadCompleteEvent(s,t);
                        i.Download();
                        i.Dispose();
                    }
                    Thread.Sleep(10);
                }
            }
            catch (ThreadAbortException)
            {
            }
        }
    }
}
