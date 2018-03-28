using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Loki.SpeechControl.Recognition
{
    public class MicrosoftRecognizer : IRecognizer
    {
        public Action<RecognitionResult> OnRecognized { get; set; }
        private readonly SpeechRecognitionEngine Recognizer;


        public MicrosoftRecognizer()
        {
            // Create the recognition engine
            Recognizer = new SpeechRecognitionEngine(new CultureInfo("en-US"));

            // Configure input to the speech recognizer.
            Recognizer.SetInputToDefaultAudioDevice();

            // Add a handler for the speech recognized event.
            Recognizer.SpeechRecognized += OnSpeechRecognized;
        }





        public void LoadGrammar(Grammar grammar)
        {
            Recognizer.LoadGrammar(grammar);
        }

        public void LoadGrammar(IEnumerable<Grammar> grammars)
        {
            foreach (Grammar grammar in grammars)
                LoadGrammar(grammar);
        }

        public void Start()
        {
            Recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Pause()
        {
            Recognizer.RecognizeAsyncCancel();
        }
        



        private void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            OnRecognized(e.Result);
        }




        public void Dispose()
        {
            Recognizer.Dispose();
        }
    }
}
