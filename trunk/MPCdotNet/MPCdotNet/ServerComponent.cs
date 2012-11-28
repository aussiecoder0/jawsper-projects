using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace MPCdotNet
{
    internal class ServerComponent
    {
        private string m_Host;
        private int m_Port;
        private const int READ_BUFFER_SIZE = 1024;
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader r;
        private StreamWriter w;

        internal ServerComponent(string host, int port)
        {
            m_Host = host;
            m_Port = port;
        }

        internal bool Open()
        {
            try
            {
                // connect to the server
                client = new TcpClient(m_Host, m_Port);
                stream = client.GetStream();

                // init the reader with UTF8
                r = new StreamReader(stream, Encoding.UTF8);
                r.BaseStream.ReadTimeout = 5000;

                // init the writer with UTF8 without BOM, and \n as newline
                w = new StreamWriter(stream, new UTF8Encoding(false));
                w.NewLine = "\n";
                ReadLine();
                return true;
            }
            catch( Exception )
            {
                return false;
            }
        }
        internal void Close()
        {
            if (stream == null) return;
                // say byebye to the server
                WriteLine("close");
                // closing this will kill the reader/writer and client
                // this has a timeout to be able to flush the buffer
                stream.Close(5000);

            // garbage in, garbage out
            r = null;
            w = null;
            stream = null;
            client = null;
        }

        internal List<KeyValuePair<string,string>> SendCommand(string cmd, params string[] args)
        {
            if (client == null) return null;

            if (args != null && args.Length > 0)
            {
                cmd += " " + string.Join(" ", args.Select(a => a.Contains(" ") ? " " + a + " " : a));
            }
            WriteLine(cmd);

            var list = new List<KeyValuePair<string, string>>();

            while (true)
            {
                var line = ReadLine();
                if (IsError(line)) { throw new MPCException(line); }
                if (line == "OK") break;

                var kv = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
                if (kv.Length == 2)
                {
                    list.Add(new KeyValuePair<string,string>(kv[0],kv[1]));
                }

            }

            return list;
        }

        internal List<KeyValuePair<string, string>> ExecuteCommands(params string[][] commands)
        {
            if (client == null) return null;

            WriteLine("command_list_begin");

            foreach (var command in commands)
            {
                var str = command[0];
                if (command.Length > 1)
                {
                    for (var i = 1; i < command.Length; i++)
                    {
                        if (command[i].Length < 1) continue;
                        str += " ";
                        if (command[i].Contains(" ")) str += "\"" + command[i] + "\"";
                        else str += command[i];
                    }
                }
                WriteLine(str);
            }

            return SendCommand( "command_list_end" );
        }

        private void WriteLine(string s)
        {
            Debug.WriteLine(string.Format("[TX] {0}", s));
            w.WriteLine(s);
            w.Flush();
        }

        private string ReadLine(bool idle = false)
        {
            string l;
            if (idle)
            {
                var old_timeout = stream.ReadTimeout;
                stream.ReadTimeout = System.Threading.Timeout.Infinite;
                l = r.ReadLine();
                stream.ReadTimeout = old_timeout;
            }
            else
            {
                l = r.ReadLine();
            }
            Debug.WriteLine(string.Format("[RX] {0}", l));
            return l;
        }

        private bool IsError(string line)
        {
            return line.StartsWith("ACK ");
        }
    }
}
