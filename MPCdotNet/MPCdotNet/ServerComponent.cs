using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MPCdotNet
{
    internal class ServerComponent
    {
        private const int READ_BUFFER_SIZE = 1024;
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader r;
        private StreamWriter w;

        internal ServerComponent()
        {
        }

        internal void Open(string server, int port)
        {
            // connect to the server
            client = new TcpClient(server, port);
            stream = client.GetStream();

            // init the reader with UTF8
            r = new StreamReader(stream, Encoding.UTF8);
            r.BaseStream.ReadTimeout = 5000;

            // init the writer with UTF8 without BOM, and \n as newline
            w = new StreamWriter(stream, new UTF8Encoding(false));
            w.NewLine = "\n";
            Console.WriteLine("[RX] {0}", r.ReadLine());
        }
        internal void Close()
        {
            // say byebye to the server
            WriteLine("close");
            // closing this will kill the reader/writer and client
            // this has a timeout to be able to flush the buffer
            stream.Close(5000);
        }

        internal List<string[]> GetList(string cmd)
        {
            WriteLine(cmd);

            var list = new List<string[]>();

            while (true)
            {
                var line = r.ReadLine();
                if (IsError(line)) { throw new MPCException(line); }
                if (line == "OK") break;

                var kv = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                if (kv.Length > 0)
                {
                    list.Add(kv);
                }

            }

            return list;
        }

        internal void WriteLine(string s)
        {
            Console.WriteLine("[TX] {0}", s);
            w.WriteLine(s);
            w.Flush();
        }

        internal string ReadLine()
        {
            return r.ReadLine();
        }

        internal bool IsError(string line)
        {
            return line.StartsWith("ACK ");
        }
    }
}
