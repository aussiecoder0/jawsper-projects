using System;
using System.Text.RegularExpressions;

namespace MPCdotNet
{
    public class MPCException : Exception
    {
        int error;
        int command_listNum;
        string current_command;
        string message_text;

        internal MPCException(string line)
        {
            if (line == null)
            {
                message_text = "Disconnected";
                return;
            }
            var rx = new Regex(@"^ACK \[(\d+)@(\d+)\] {([^}]*)} (.+)$");
            var m = rx.Match(line);
            if (m.Success)
            {
                error = int.Parse(m.Groups[1].Value);
                command_listNum = int.Parse(m.Groups[2].Value);
                current_command = m.Groups[3].Value;
                message_text = m.Groups[4].Value;
            }
            else
            {
                message_text = line;
            }
        }
        public override string Message
        {
            get
            {
                return string.Format("{3}", error, command_listNum, current_command, message_text);
            }
        }
    }

}