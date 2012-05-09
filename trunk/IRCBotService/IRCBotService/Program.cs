using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace IRCBotService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (!Environment.UserInteractive)
            {
                ServiceBase.Run(new Service1());
            }
            else
            {
                var bot = new IRCBot();
                new Thread(new ThreadStart(bot.Start)).Start();
                Console.ReadLine();
                bot.Stop();
            }
        }
    }
}
