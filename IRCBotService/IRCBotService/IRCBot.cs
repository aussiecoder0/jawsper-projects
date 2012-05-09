using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace IRCBotService
{
    class IRCBot
    {
        private Socket m_Socket;
        private NetworkStream m_NetworkStream;
        private StreamWriter m_StreamWriter;
        private StreamReader m_StreamReader;

        public void Start()
        {
            Console.WriteLine("Bot is starting");

            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.Connect("192.168.1.5", 31337);
            m_NetworkStream = new NetworkStream(m_Socket, false);
            m_StreamWriter = new StreamWriter(m_NetworkStream);
            m_StreamReader = new StreamReader(m_NetworkStream);
            ReadLine();
        }

        public void Stop()
        {
            Console.WriteLine("Bot is stopping");
            m_StreamReader.Close();
            m_StreamWriter.Close();
            m_NetworkStream.Close();
            m_Socket.Close();
        }

        private void WriteLine(string line)
        {
            m_StreamWriter.WriteLine(line);
            Console.WriteLine(">>> {0}", line);
        }
        private string ReadLine()
        {
            try
            {
                string line = m_StreamReader.ReadLine();
                Console.WriteLine("<<< {0}", line);
                return line;
            }
            catch { return null; }
        }
    }
}
