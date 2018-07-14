using System;
using System.Collections.Generic;
using System.Text;

namespace ProArch.CodingTest.ServiceManager
{
    public class OfflineLogger : ILogger, IOffline
    {
        public IOnline Online { get; set; }

        public void Write(string message)
        {
            Console.WriteLine("Offline Logger: " + message);
        }
    }

}
