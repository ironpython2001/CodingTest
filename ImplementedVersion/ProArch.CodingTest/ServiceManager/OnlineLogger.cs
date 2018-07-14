using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.ServiceManager
{
    public class OnlineLogger : ILogger, IOnline
    {
        public IOffline Offline { get; set; }

        public OnlineLogger()
        {
            this.Offline = new OfflineLogger();
        }

        public void Write(string message)
        {
            Console.WriteLine("Online Logger: " + message);
        }
    }

}
