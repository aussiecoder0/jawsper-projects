using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace jawsper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var start = DateTime.Now;
            using (var stream1 = new FileStream(@"D:\Backups\Windows\2012-01-10\Users\Jawsper\Downloads\SABnzbd-0.6.10-src.tar.gz", FileMode.Open))
            using(var stream2 = new GZipStream(stream1,CompressionMode.Decompress))
            {
                OpenTar(stream2);
            }
            var end = DateTime.Now;
            Console.WriteLine("Took me: {0}", end - start);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }


        private static void OpenTar(Stream file)
        {
            using (var tar = new CsTar(file))
            {
                tar.ReadHeaders();
            }
        }
    }
}
