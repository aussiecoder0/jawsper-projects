using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace DownloadManager
{
    class DownloadItem : IDisposable
    {
        public event DownloadStartEventHandler DownloadStartEvent;
        public event DownloadProgressEventHandler DownloadProgressEvent;
        public event DownloadCompleteEventHandler DownloadCompleteEvent;

        private Uri m_Uri;
        private string m_Referer;
        private DirectoryInfo m_Destination;

        public DownloadItem(Uri a_Uri, DirectoryInfo a_Destination)
        {
            m_Uri = a_Uri;
            m_Destination = a_Destination;
        }

        public string Referer { set { m_Referer = value; } }

        public FileInfo GetDestinationFile()
        {
             return new FileInfo(m_Destination.FullName + Path.DirectorySeparatorChar + Path.GetFileName(m_Uri.GetLeftPart(UriPartial.Path)));
        }

        public void Download()
        {
            DateTime start = DateTime.Now;


            var filename = GetDestinationFile();

            DownloadStartEvent(filename.Name);
            HttpWebRequest r = (HttpWebRequest)HttpWebRequest.Create(m_Uri);
            if (m_Referer != null)
            {
                r.Referer = m_Referer;
            }
            HttpWebResponse resp = (HttpWebResponse)r.GetResponse();


            long size = resp.ContentLength;
            long downloaded = 0;
            int count = 0;
            using (var i_stream = resp.GetResponseStream())
            using (var o_stream = filename.OpenWrite())
            {
                var buff = new byte[8 * 1024];
                do
                {
                    count = i_stream.Read(buff, 0, buff.Length);
                    o_stream.Write(buff, 0, count);
                    downloaded += count;
                    DownloadProgressEvent(downloaded, -1);
                } while (count > 0);
            }

            DownloadCompleteEvent(resp.ContentLength, DateTime.Now - start);
        }

        public void Dispose()
        {
            m_Uri = null;
            m_Destination = null;
        }
    }
}
