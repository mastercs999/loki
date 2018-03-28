using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.SpeechControl.Recognition
{
    public interface IRecognizer : IDisposable
    {
        Action<RecognitionResult> OnRecognized { get; set; }
        void LoadGrammar(Grammar grammar);
        void LoadGrammar(IEnumerable<Grammar> grammars);
        void Start();
        void Pause();
    }
}
