using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceAssistant.Commands;
using VoiceAssistant.SpeechControl.Recognition;
using VoiceAssistant.SpeechControl.Synthesis;

namespace VoiceAssistant
{
    public class Loki
    {
        public void Start()
        {
            // Define commands
            ICommand[] commands = new ICommand[]
            {
                new StartCommand("start Mozilla", @"C:\Program Files\Mozilla Firefox\firefox.exe"),

                new CommandLineCommand("shutdown", "shutdown /s /hybrid /f /t 0")
            };

            // Create grammars
            List<Grammar> grammars = commands.Select(x => new Grammar(new GrammarBuilder(x.Phrase)) { Name = x.Id }).ToList();

            // Fast searching for command
            Dictionary<string, ICommand> idToCommand = commands.ToDictionary(x => x.Id);

            using (IRecognizer recognizer = new MicrosoftRecognizer())
            using (ISynthesizer synthesizer = new MicrosoftSynthesizer(recognizer))
            {
                // Create and load a dictation grammar.
                //GrammarBuilder builder = new GrammarBuilder();
                //builder.Append("who's the boss?");
                ////builder.AppendDictation();
                ////builder.Append("weather");
                ////builder.AppendDictation();

                //recognizer.LoadGrammar(new Grammar(builder) { Name = "AA" });
                //recognizer.LoadGrammar(new DictationGrammar());

                // Load grammars
                recognizer.LoadGrammar(grammars);

                // Processing command on recognized
                recognizer.OnRecognized = (recognizedArgs) =>
                {
                    new Thread(() =>
                    {
                        idToCommand[recognizedArgs.Grammar.Name].Execute();
                    })
                    {
                        IsBackground = true
                    }.Start();
                };

                // Start
                recognizer.Start();

                // Never ends
                Thread.Sleep(int.MaxValue);
            }
        }
    }
}
