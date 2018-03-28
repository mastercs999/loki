using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Loki.SpeechControl.Synthesis
{
    public class MicrosoftSynthesizer : ISynthesizer
    {
        private readonly SpeechSynthesizer Synthesizer;




        public MicrosoftSynthesizer()
        {
            Synthesizer = new SpeechSynthesizer();

            // Configure
            Synthesizer.SelectVoiceByHints(VoiceGender.Male);
        }

        public void Speak(string text)
        {
            Synthesizer.Speak(text);
        }

        public void Dispose()
        {
            Synthesizer.Dispose();
        }
    }
}
