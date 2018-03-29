using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceAssistant;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Loki loki = new Loki();

            // Install assistant
            loki.Install();

            // Start him
            loki.Start();
        }
    }
}
