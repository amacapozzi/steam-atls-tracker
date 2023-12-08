using Gameloop.Vdf.Linq;
using Gameloop.Vdf;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Gameloop.Vdf.JsonConverter;

namespace AltsTracker.Helpers
{
    internal class SteamHelper
    {
        public class SteamObjet
        {
            public string users {  get; set; }
        }
        public static void GetSteamAvatars()
        {
            string steamPath = SearchSteamRegeditValue("SOFTWARE\\WOW6432Node\\Valve\\Steam");

            if (steamPath != null)
            {
                string avatarsPath = Path.Combine(steamPath, "config", "avatarcache");

                foreach (string file in Directory.GetFiles(avatarsPath))
                {
                    if (!Directory.Exists("avatars"))
                    {
                        Directory.CreateDirectory("avatars");
                    }

                    FileInfo fileinfo = new FileInfo(file);
                    byte[] fileBytes = File.ReadAllBytes(file);
                    string destinationPath = Path.Combine(Environment.CurrentDirectory, "avatars", fileinfo.Name);
                    File.WriteAllBytes(destinationPath, fileBytes);

                }

            }
        }

        public static List<string> GetSteamPersonaNames()
        {
            string steamPath = SearchSteamRegeditValue("SOFTWARE\\WOW6432Node\\Valve\\Steam");

            if (steamPath != null)
            {
                string steamDataPath = Path.Combine(steamPath, "config");
                string loginUsersFilePath = Path.Combine(steamDataPath, "loginusers.vdf");

                List<string> personaNames = new List<string>();

                using (StreamReader sr = new StreamReader(loginUsersFilePath))
                {
                    string line;
                    string currentUser = null;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains('{'))
                        {
                            currentUser = line.Trim('\"', ' ', '{');
                        }
                        else if (line.Contains('}'))
                        {
                            currentUser = null;
                        }
                        else if (currentUser != null)
                        {
                            var keyValue = line.Trim().Split(new[] { '\t' }, 2);
                            if (keyValue.Length == 2 && keyValue[0].Trim('\"') == "PersonaName")
                            {
                                personaNames.Add(keyValue[1].Trim('\"'));
                            }
                        }
                    }
                }

                return personaNames;
            }

            return null;
        }


        private static string SearchSteamRegeditValue(string key)
        {
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(key, true))
            {
                if (rk == null)
                {
                    return null;

                }
                else
                {
                    return rk.GetValue("InstallPath").ToString();
                }
            }
        }
    }

}

