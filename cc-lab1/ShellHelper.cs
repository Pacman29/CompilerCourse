using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace cc_lab1
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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
            else
            {
                throw new Exception($"Your system not execute this bash command: {cmd}");
            }
        }
    }
}