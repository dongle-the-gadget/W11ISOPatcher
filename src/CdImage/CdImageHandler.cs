using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CdImage
{
    public static class CDImage
    {
        public delegate void ProgressCallback(string Operation, int ProgressPercentage, bool IsIndeterminate);


        public static bool GenerateISOImage(string isopath, string cdroot, string volumelabel, ProgressCallback progressCallback)
        {
            string setupexe = Path.Combine(cdroot, "setup.exe");
            DateTime creationtime = File.GetCreationTimeUtc(setupexe);


            try
            {
                foreach (string entry in Directory.EnumerateFileSystemEntries(cdroot, "*", SearchOption.AllDirectories))
                {
                    if (Directory.Exists(entry))
                    {
                        try
                        {
                            Directory.SetCreationTimeUtc(entry, creationtime);
                        }
                        catch { }
                        try
                        {
                            Directory.SetLastAccessTimeUtc(entry, creationtime);
                        }
                        catch { }
                        try
                        {
                            Directory.SetLastWriteTimeUtc(entry, creationtime);
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            File.SetCreationTimeUtc(entry, creationtime);
                        }
                        catch { }
                        try
                        {
                            File.SetLastAccessTimeUtc(entry, creationtime);
                        }
                        catch { }
                        try
                        {
                            File.SetLastWriteTimeUtc(entry, creationtime);
                        }
                        catch { }
                    }
                }


                string cmdline = $"-b \"boot/etfsboot.com\" --no-emul-boot --eltorito-alt-boot -b \"efi/microsoft/boot/efisys.bin\" --no-emul-boot --udf --hide \"*\" -V \"{volumelabel}\" -o \"{isopath}\" {cdroot}";


                ProcessStartInfo processStartInfo = new("mkisofs",
                    cmdline);


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
                            int percent = (int)Math.Round(double.Parse(e.Data.Split(' ').First(x => x.Contains("%")).Replace("%", "")));
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
            catch
            {
                return false;
            }
        }
    }
}
