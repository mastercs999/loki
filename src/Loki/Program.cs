using Loki.SpeechControl.Recognition;
using Loki.SpeechControl.Synthesis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Loki
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IRecognizer recognizer = new MicrosoftRecognizer())
            using (ISynthesizer synthesizer = new MicrosoftSynthesizer())
            {
                // Create and load a dictation grammar.
                GrammarBuilder builder = new GrammarBuilder();
                builder.Append("who's the boss?");
                //builder.AppendDictation();
                //builder.Append("weather");
                //builder.AppendDictation();

                recognizer.LoadGrammar(new Grammar(builder) { Name = "AA" });
                recognizer.LoadGrammar(new DictationGrammar());

                // Init
                recognizer.OnRecognized = (recognizedArgs) =>
                {
                    recognizer.Pause();

                    Console.WriteLine(recognizedArgs.Text);
                    synthesizer.Speak("You said " + recognizedArgs.Text);

                    recognizer.Start();
                };
               
                // Start
                recognizer.Start();

                // Keep the console window open.
                while (true)
                    Console.ReadLine();
            }
        }
    }
}
