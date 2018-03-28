using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands
{
    public abstract class Command
    {
        public string Id { get; private set; }

        public Command()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
