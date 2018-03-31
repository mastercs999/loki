using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands
{
    public class CloseCommand : Command, ICommand
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        public string Phrase { get; }



        public CloseCommand(string phrase)
        {
            Phrase = phrase;
        }




        public void Execute(string said)
        {
            GetWindowThreadProcessId(GetForegroundWindow(), out int processID);

            Process.GetProcessById(processID)?.CloseMainWindow();
        }
    }
}
