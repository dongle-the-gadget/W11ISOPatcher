using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CdImage
{
    public static class PowerShellMount
    {
        /// <summary>
        /// Mount an ISO file.
        /// </summary>
        /// <param name="isoFile">ISO file (absolute path).</param>
        /// <returns>Drive letter</returns>
        public static string Mount(string isoFile)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "powershell.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.Arguments = $"-Command \"(Mount-DiskImage '{isoFile}' -PassThru | Get-Volume).DriveLetter\"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.WaitForExit();
            string output = proc.StandardOutput.ReadToEnd();
            return output.Replace(Environment.NewLine, "");
        }

        /// <summary>
        /// Dismount an ISO file.
        /// </summary>
        /// <param name="isoFile">ISO file (absolute path).</param>
        public static void Dismount(string isoFile)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "powershell.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.Arguments = $"-Command \"Dismount-DiskImage -ImagePath '{isoFile}'\"";
            proc.Start();
            proc.WaitForExit();
        }
    }
}
