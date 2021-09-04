// See https://aka.ms/new-console-template for more information
using CdImage;
using System.Runtime.InteropServices;

string[] help = new string[] { "/?", "-?", "--?", "-help", "--help" };
string[] appraiserressource = new string[] { "auto", "offline", "online", "online-x64", "online-arm64" };

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    Console.WriteLine("This program does not support Windows.");
    return;
}

if (args.Any(f => help.Contains(f.ToLower())) 
    || args.Length < 3 
    || args.Length > 4 
    || (args.Length == 4 && appraiserressource.Any(f => f == args[3].ToLower())))
{
    Console.WriteLine($"Usage: W11ISO <Source ISO> <Destination ISO> <Working Directory> [auto|offline|online|online-x64|online-arm64]");
    return;
}

string isoPath = args[0];
string finalPath = args[3];
string workingDir = args[2];

Directory.CreateDirectory(Path.Combine(workingDir, "isoroot"));

if (!SevenZipExtract.Extract(isoPath, Path.Combine(workingDir, "isoroot")))
{

}