using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using VoiceAssistant.SpeechControl.Recognition;

namespace VoiceAssistant.SpeechControl.Synthesis
{
    public class MicrosoftSynthesizer : ISynthesizer
    {
        private readonly SpeechSynthesizer Synthesizer;
        private readonly IRecognizer Recognizer;



        public MicrosoftSynthesizer() : this(null)
        {

        }
        public MicrosoftSynthesizer(IRecognizer recognizer)
        {
            Synthesizer = new SpeechSynthesizer();
            Recognizer = recognizer;

            // Configure
            Synthesizer.SelectVoiceByHints(VoiceGender.Male);
        }

        public void Speak(string text)
        {
            Recognizer?.Pause();

            Synthesizer.Speak(text);

            Recognizer?.Start();
        }

        public void Dispose()
        {
            Synthesizer.Dispose();
        }
    }
}
