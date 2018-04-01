using Microsoft.Win32;
using PrimS.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands.Vlc
{
    /// <summary>
    /// Description of possible commands here: https://wiki.videolan.org/Talk:Console/
    /// </summary>
    public class VlcCommand : Command, ICommand
    {
        public string Phrase { get; }

        private readonly static string TelnetPassword = "loki";
        private readonly VlcCommandType CommandType;



        public VlcCommand(string command, VlcCommandType commandType)
        {
            Phrase = command;
            CommandType = commandType;
        }




        public void Execute(string said)
        {
            try
            {
                using (Client client = new Client("localhost", 4212, new CancellationToken()))
                {
                    client.TryLoginAsync(null, TelnetPassword, 5000).Wait();

                    if (CommandType == VlcCommandType.Simple)
                        client.WriteLine(Phrase);
                    else if (CommandType == VlcCommandType.Skip)
                    {
                        // Find out current position of a movie
                        client.WriteLine("get_time");
                        int currentSeconds = int.Parse(CleanResponse(client.ReadAsync().Result.Trim()));

                        string[] parts = said.Split(null);
                        int value = int.Parse(parts[1]);
                        int multiplier = parts[2].Contains("second") ? 1 : (parts[2].Contains("minute") ? 60 : 3600);

                        client.WriteLine($"seek {currentSeconds + value * multiplier}");
                    }
                }
            }
            catch (SocketException ex)
            {
                // TODO: log
            }
        }

        private string CleanResponse(string response)
        {
            return response.Substring(0, response.IndexOf('>')).Trim();
        }




        public static void Install()
        {
            // Make sure VLC starts with telnet
            foreach (string vlcEntry in Registry.ClassesRoot.GetSubKeyNames().Where(x => x.StartsWith("VLC.")))
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(vlcEntry + @"\shell\Open\command", true);
                if (key == null)
                    continue;

                object value = key.GetValue(null);
                if (!(value is string str) || !str.ToLower().Contains("vlc.exe"))
                    continue;

                string newArguments = "--intf=qt --extraintf telnet --telnet-password " + TelnetPassword;
                if (str.Contains("extraintf") && str.Contains("telnet"))
                    continue;

                // Set new argument to path
                int insertPosition = str.Substring(1).IndexOf("\"");
                string newValue = str.Substring(0, insertPosition + 2) + " " + newArguments + " " + str.Substring(insertPosition + 2);
                key.SetValue(null, newValue);
            }
        }
    }
}
