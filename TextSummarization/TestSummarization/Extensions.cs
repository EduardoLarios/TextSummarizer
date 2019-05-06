using System;
using System.Linq;
using System.Diagnostics;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static bool ContainsWord(this string sentence, string searched)
        {
            var trim = new char[] { ',', '"', '\'' };
            var words = sentence.Split('\n').Select(w => w.Trim(trim).Replace("\r", ""));
            foreach (var word in words)
            {
                if(string.Equals(word, searched)) return true;
            }

            return false;
        }
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                }
            };
            
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}