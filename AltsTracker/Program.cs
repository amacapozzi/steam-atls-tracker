using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltsTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Helpers.WriteHelepr.WriteLine($"{{FC={ConsoleColor.Green}}}[+]{{/FC}} Welcome to Alts Tracker\n");
            Helpers.SteamHelper.GetSteamAvatars();

            List<string> allPersonaNames = Helpers.SteamHelper.GetSteamPersonaNames();

            if (allPersonaNames != null)
            {
                foreach (string name in allPersonaNames)
                {
                    Console.WriteLine(name.Replace('"', ' ').Trim());
                }
            }

            Console.ReadLine();


        }
    }
}
