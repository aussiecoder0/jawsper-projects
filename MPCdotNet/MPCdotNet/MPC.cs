using System;
using System.Collections.Generic;

namespace MPCdotNet
{
    public class MPC
    {
        private ServerComponent s;

        public MPC()
        {
            s = new ServerComponent();
        }

        public void Open(string server, int port)
        {
            s.Open(server,port);
        }
        public void Close()
        {
            s.Close();
        }

        public List<string[]> GetStatus()
        {
            return s.GetList("status");
        }
        public List<string[]> GetStats()
        {
            return s.GetList("stats");
        }
        public List<Output> GetOutputs()
        {
            var list = s.GetList("outputs");
            var outputs = new List<Output>();
            int id = 0; string name = "";
            foreach (var kv in list)
            {
                if (kv[0] == "outputid") id = int.Parse(kv[1]);
                else if (kv[0] == "outputname") name = kv[1];
                else
                {
                    outputs.Add(new Output(id, name, kv[1] == "1"));
                }
            }
            return outputs;
        }
        public List<string[]> GetCommands()
        {
            return s.GetList("commands");
        }
        public List<string[]> GetNotCommands()
        {
            return s.GetList("notcommands");
        }

        public List<string[]> GetCurrentSong()
        {
            return s.GetList("currentsong");
        }

        public bool SendCommand(string command, string args)
        {
            return SendCommand(command + " " + args);
        }
        public bool SendCommand( string command )
        {
            try
            {
                var response = s.GetList( command );
                return true;
            }
            catch( MPCException mpe )
            {
                Console.WriteLine("Error: " + mpe.Message);
                return false;
            }
        }
    }
}
