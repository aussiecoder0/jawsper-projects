using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace nl.jawsper.NiceFileCopy
{
    class Queue : Thread
    {

    }

    class QueueItem
    {
        private FileInfo source;
        private FileInfo destination;

        public void run()
        {
            using(var fs = source.OpenRead())
            using (var fd = destination.OpenWrite())
            {
                var buff = new byte[1024*1024];
                while (fs.Position < fs.Length)
                {
                    int c = fs.Read(buff, 0, buff.Length);
                    fd.Write(buff, 0, c);
                }
            }
            destination.Attributes = source.Attributes;
        }
    }
}
