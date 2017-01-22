using System;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace LeyStoneEngine.Utility
{
    public static class Logger
    {
        private static string path = "";

        public static void CreateNewLogFile()
        {
            string fileName = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);

            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            File.Create("Logs/" + fileName);

            path = "Logs/" + fileName;
        }
        public static void Log(string text, bool print)
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            if (!File.Exists(path))
                File.Create(path);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(text);
                sw.Close();
            }

            if (print)
                Console.WriteLine("Logged: " + text);
        }
    }
}
