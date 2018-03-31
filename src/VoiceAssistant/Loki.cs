using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VoiceAssistant.Commands;
using VoiceAssistant.Exceptions;
using VoiceAssistant.Helpers;
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
                new OpenCommand("open Mozilla", @"C:\Program Files\Mozilla Firefox\firefox.exe"),
                new CloseCommand("close"),

                //new VlcCommand("pause"),
                //new VlcCommand("play"),
                new KeyboardCommand("pause", System.Windows.Input.Key.Space),
                new KeyboardCommand("play", System.Windows.Input.Key.Space),

                //new CommandLineCommand("shutdown", "shutdown /s /hybrid /f /t 0")
            };

            // Create grammars
            List<Grammar> grammars = commands.Select(x => new Grammar(new GrammarBuilder(x.Phrase)) { Name = x.Id }).ToList();

            // Fast searching for command
            Dictionary<string, ICommand> idToCommand = commands.ToDictionary(x => x.Id);

            using (IRecognizer recognizer = new MicrosoftRecognizer())
            using (ISynthesizer synthesizer = new MicrosoftSynthesizer(recognizer))
            {
                // Load grammars
                recognizer.LoadGrammar(grammars);

                // Add free grammar to prevent false positive - other option is to decrease mic sensitivity
                recognizer.LoadGrammar(new DictationGrammar() { Name = "___" });

                // Processing command on recognized
                recognizer.OnRecognized = (recognizedArgs) =>
                {
                    Console.WriteLine(recognizedArgs.Text + "\t" + recognizedArgs.Confidence + "\t" + recognizedArgs.Grammar.Name);

                    // Ignore not very accurate commands
                    if (recognizedArgs.Grammar.Name == "___")
                        return;

                    new Thread(() =>
                    {
                        idToCommand[recognizedArgs.Grammar.Name].Execute(recognizedArgs.Text);
                    })
                    {
                        IsBackground = true
                    }.Start();
                };

                // Start
                recognizer.Start();

                // Never ends
                Console.ReadLine();
                Thread.Sleep(int.MaxValue);
            }
        }

        public void Install()
        {
            // Check running as administrator
            if (!Utilities.IsAdministrator())
                throw new UnsufficientPrivileges("You must run this application as an administrator");

            VlcCommand.Install();
        }
    }
}
