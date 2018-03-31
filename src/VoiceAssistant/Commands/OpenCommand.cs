using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands
{
    public class OpenCommand : Command, ICommand
    {
        public string Phrase { get; }

        private readonly string TargetPath;




        public OpenCommand(string phrase, string targetPath)
        {
            Phrase = phrase;
            TargetPath = targetPath;
        }




        public void Execute(string said)
        {
            Process.Start(TargetPath);
        }
    }
}
