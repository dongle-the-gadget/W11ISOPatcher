using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CdImage
{
    public class CdImageHandler
    {
        public delegate void ProgressCallback(string Operation, int ProgressPercentage, bool IsIndeterminate);

        public static bool GenerateISOImage(string isopath, string cdroot, string volumelabel, ProgressCallback progressCallback)
        {
            string setupexe = Path.Combine(cdroot, "setup.exe");
            DateTime creationtime = File.GetCreationTimeUtc(setupexe);


            string timestamp = creationtime.ToString("MM/dd/yyyy,hh:mm:ss");


            ProcessStartInfo processStartInfo = new("cdimage.exe",
                $"\"-bootdata:2#p0,e,b{cdroot}\\boot\\etfsboot.com#pEF,e,b{cdroot}\\efi\\Microsoft\\boot\\efisys.bin\" -o -h -m -u2 -udfver102 -t{timestamp} -l{volumelabel}  \"{cdroot}\" \"{isopath}\"");


            processStartInfo.UseShellExecute = false;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.CreateNoWindow = true;


            Process process = new();
            process.StartInfo = processStartInfo;


            try
            {
                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data?.Contains("%") == true)
                    {
                        int percent = int.Parse(e.Data.Split(' ').First(x => x.Contains("%")).Replace("%", ""));
                        progressCallback?.Invoke($"Building {isopath}", percent, false);
                    }
                };
                process.Start();
                process.BeginErrorReadLine();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
