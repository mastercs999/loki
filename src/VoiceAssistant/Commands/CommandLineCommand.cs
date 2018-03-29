using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands
{
    public class CommandLineCommand : Command, ICommand
    {
        [DllImport("Kernel32.Dll", EntryPoint = "Wow64EnableWow64FsRedirection")]
        private static extern bool EnableWow64FSRedirection(bool enable);

        public string Phrase { get; }

        private readonly string Command;




        public CommandLineCommand(string phrase, string command)
        {
            Phrase = phrase;
            Command = command;
        }

        public void Execute(string said)
        {
            // We need to disable redirection otherwise some things like telnet doesn't work
            EnableWow64FSRedirection(false);

            // Set up process
            ProcessStartInfo processInfo = new ProcessStartInfo(@"cmd.exe", $"/c {Command}")
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            // Start the process
            Process.Start(processInfo);

            // Enable redirection again
            EnableWow64FSRedirection(true);
        }
    }
}
