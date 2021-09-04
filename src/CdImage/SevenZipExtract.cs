using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CdImage
{
    public static class SevenZipExtract
    {
        public static bool Extract(string extractFile, string extractFolder)
        {
            Process process = new Process();
            process.StartInfo.FileName = "7z";
            process.StartInfo.Arguments = $"x \"{extractFile}\"";
            process.StartInfo.WorkingDirectory = extractFolder;
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0 || process.ExitCode == 1;
        }
    }
}
