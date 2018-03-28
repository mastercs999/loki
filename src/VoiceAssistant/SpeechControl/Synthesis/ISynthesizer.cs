using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.SpeechControl.Synthesis
{
    public interface ISynthesizer : IDisposable
    {
        void Speak(string text);
    }
}
