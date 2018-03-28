using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands
{
    public interface ICommand
    {
        string Id { get; }
        string Phrase { get; }

        void Execute();
    }
}
