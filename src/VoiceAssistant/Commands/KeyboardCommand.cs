using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VoiceAssistant.Commands
{
    public class KeyboardCommand : Command, ICommand
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public string Phrase { get; }

        private readonly Key Key;


        public KeyboardCommand(string phrase, Key key)
        {
            Phrase = phrase;
            Key = key;
        }




        public void Execute(string said)
        {
            IntPtr windowHandle = GetForegroundWindow();

            uint virtualKey = (uint)KeyInterop.VirtualKeyFromKey(Key);
            int lDown = 1;
            int lUp = 1 << 31 | 1;

            // Keydown message
            PostMessage(windowHandle, 0x0100, (IntPtr)virtualKey, (IntPtr)lDown);

            Thread.Sleep(200);

            // Keyup message
            PostMessage(windowHandle, 0x0101, (IntPtr)virtualKey, (IntPtr)lUp);
        }
    }
}
