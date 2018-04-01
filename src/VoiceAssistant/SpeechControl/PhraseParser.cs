using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.SpeechControl
{
    public static class PhraseParser
    {
        public static GrammarBuilder Parse(string phrase)
        {
            GrammarBuilder builder = new GrammarBuilder();

            foreach (string word in phrase.Split(null))
            {
                // Handle multiple possibilities
                if (word.StartsWith("[") && word.EndsWith("]"))
                    builder.Append(Parse(word.Trim('[', ']')));
                else if (word.Contains("|"))
                    builder.Append(new Choices(word.Split('|').Select(x => Parse(x)).ToArray()));
                else if (word.Contains("-"))
                {
                    // Handle range number
                    int start = int.Parse(word.Split(':')[0]);
                    int end = int.Parse(word.Split(':')[1]);

                    builder.Append(new Choices(Enumerable.Range(start, end - start + 1).Select(x => x.ToString()).ToArray()));
                }
                else
                    builder.Append(word);
            }

            return builder;
        }
    }
}
