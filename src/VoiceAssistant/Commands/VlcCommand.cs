using Microsoft.Win32;
using PrimS.Telnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceAssistant.Commands
{
    /// <summary>
    /// Description of possible commands here: https://wiki.videolan.org/Talk:Console/
    /// </summary>
    public class VlcCommand : Command, ICommand
    {
        public string Phrase { get; }

        private readonly static string TelnetPassword = "loki";



        public VlcCommand(string command)
        {
            Phrase = command;
        }




        public void Execute(string said)
        {
            try
            {
                using (Client client = new Client("localhost", 4212, new CancellationToken()))
                {
                    client.TryLoginAsync(null, TelnetPassword, 5000).Wait();
                    client.WriteLine(Phrase);
                }
            }
            catch (SocketException ex)
            {
                // TODO: log
            }
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
